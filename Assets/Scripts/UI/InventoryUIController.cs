using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Persistence;
using Persistence.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIController : MonoBehaviour
{
    public static readonly ICollection<Action<bool>> OnInventoryToggleCallbacks = new List<Action<bool>>();
    
    private StatRepository _statRepository;
    private InventoryRepository _inventoryRepository;

    private readonly List<InventorySlot> _inventorySlots = new List<InventorySlot>();
    private readonly List<TraderSlot> _traderSlots = new List<TraderSlot>();

    private VisualElement _mRoot;
    private VisualElement _mSlotContainer;
    private VisualElement _traderInfo;
    private Label _traderHeader;
    private Label _moneyLabel;
    private VisualElement _traderSlotContainer;

    private static VisualElement _mGhostIcon;

    private static bool _isVisible;
    private static bool _isDragging;
    private static InventorySlot _originalSlot;

    private ICollection<InventoryItem> _inventoryItems;

    private static bool _isTradeMode;
    [CanBeNull] private Trader _trader = null;

    private void Awake()
    {
        //Store the root from the UI Document component
        _mRoot = GetComponent<UIDocument>().rootVisualElement;
        _mRoot.style.display = DisplayStyle.None;

        //Search the root for the SlotContainer Visual Element
        _mSlotContainer = _mRoot.Q<VisualElement>("SlotContainer");
        _mGhostIcon = _mRoot.Q<VisualElement>("GhostIcon");
        _traderInfo = _mRoot.Q<VisualElement>("TraderInfo");
        _traderHeader = _mRoot.Q<Label>("TraderHeader");
        _traderSlotContainer = _mRoot.Q<VisualElement>("TraderSlotContainer");
        _moneyLabel = _mRoot.Q<Label>("MoneyDisplay");
        
        //Create InventorySlots and add them as children to the SlotContainer
        for (var i = 0; i < 20; i++)
        {
            var item = new InventorySlot();
            item.TradeCallbacks.Add(OnTrade);

            _inventorySlots.Add(item);

            _mSlotContainer.Add(item);
        }

        for (var i = 0; i < 4; i++)
        {
            var traderSlot = new TraderSlot();
            _traderSlots.Add(traderSlot);
            _traderSlotContainer.Add(traderSlot);
        }

        InventoryRepository.OnInventoryChanged.Add(GameController_OnInventoryChanged);
        _inventoryRepository = new InventoryRepository();
        StatRepository.OnIntValueChanged[Stats.Money].Add(OnMoneyChanged);
        _statRepository = new StatRepository();

        _mGhostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        _mGhostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    private void Update()
    {
        if (!Input.GetKeyUp(KeyCode.E)) return;

        _mRoot.style.display = _isVisible ? DisplayStyle.None : DisplayStyle.Flex;
        _isVisible = !_isVisible;
        
        foreach (var cb in OnInventoryToggleCallbacks)
        {
            cb.Invoke(_isVisible);
        }

        if (!_isTradeMode) return;

        _isTradeMode = false;
        _trader = null;
        _traderInfo.visible = false;
        foreach (var slot in _traderSlots)
        {
            slot.Label.text = null;
        }

        foreach (var slot in _inventorySlots)
        {
            slot.IsTradeMode = false;
        }
    }

    private void GameController_OnInventoryChanged(ICollection<InventoryItem> items)
    {
        _inventoryItems = items;

        foreach (var slot in _inventorySlots)
        {
            slot.DropItem();
        }

        //Loop through each item and if it has been picked up, add it to the next empty slot
        foreach (var item in items)
        {
            var slot = _inventorySlots[item.position];

            slot.HoldItem(item);
        }
    }

    private void OnMoneyChanged(int money)
    {
        _moneyLabel.text = money.ToString();
    }

    public static void StartDrag(Vector2 position, InventorySlot originalSlot)
    {
        if (_isTradeMode) return;

        //Set tracking variables
        _isDragging = true;
        _originalSlot = originalSlot;

        _mGhostIcon.Clear();
        //Set the new position
        _mGhostIcon.style.top = position.y - _mGhostIcon.layout.height / 2;
        _mGhostIcon.style.left = position.x - _mGhostIcon.layout.width / 2;

        //Set the image
        _mGhostIcon.Add(new Label(originalSlot.Item?.item.shortName)
        {
            style =
            {
                color = new StyleColor(new Color(255, 255, 255))
            }
        });

        //Flip the visibility on
        _mGhostIcon.style.visibility = Visibility.Visible;
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (_isTradeMode) return;

        //Only take action if the player is dragging an item around the screen
        if (!_isDragging) return;

        //Set the new position
        _mGhostIcon.style.top = evt.position.y - _mGhostIcon.layout.height / 2;
        _mGhostIcon.style.left = evt.position.x - _mGhostIcon.layout.width / 2;
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (_isTradeMode) return;

        if (!_isDragging) return;

        //Check to see if they are dropping the ghost icon over any inventory slots.
        var slots = _inventorySlots.Where(x =>
            x.worldBound.Overlaps(_mGhostIcon.worldBound)).ToList();

        var item = _inventoryItems.First(i => i.idItem == _originalSlot.Item!.idItem);
        //Found at least one
        if (slots.Count != 0)
        {
            var closestSlot = slots.OrderBy(x => Vector2.Distance
                (x.worldBound.position, _mGhostIcon.worldBound.position)).First();

            //Set the new inventory slot with the data
            closestSlot.HoldItem(item);

            //Clear the original slot
            _originalSlot.DropItem();
        }
        //Didn't find any (dragged off the window)
        else
        {
            _originalSlot.HoldItem(item);
        }

        //Clear dragging related visuals and data
        _isDragging = false;
        _originalSlot = null;
        _mGhostIcon.style.visibility = Visibility.Hidden;
    }

    public void StartTrade(Trader trader)
    {
        _mRoot.visible = true;
        _isTradeMode = true;
        _trader = trader;

        _traderInfo.visible = true;
        _traderHeader.text = trader.name;

        foreach (var (item, idx) in trader.items.Select((i, idx) => (i, idx)))
        {
            _traderSlots[idx].Item = item;
            _traderSlots[idx].Label.text = $"{item.item.shortName} - {item.price}";
        }

        foreach (var slot in _inventorySlots)
        {
            slot.IsTradeMode = true;
        }
    }

    private void OnTrade(InventoryItem item)
    {
        _inventoryRepository.RemoveItems(new Dictionary<int, int> { { item.idItem, 1 } });

        var profit = _trader!.items.First(i => i.idItem == item.idItem).price;
        _statRepository.SetIntValue(Stats.Money, _statRepository.GetIntValue(Stats.Money) + profit);
    }
}