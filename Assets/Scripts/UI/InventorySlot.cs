using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Persistence.Models;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class InventorySlot : VisualElement
{
    [CanBeNull] public InventoryItem Item;

    public Label Label;

    private Image _icon;

    public bool IsTradeMode;
    public ICollection<Action<InventoryItem>> TradeCallbacks = new List<Action<InventoryItem>>();

    public InventorySlot()
    {
        //Create a new Image element and add it to the root
        _icon = new Image();
        Label = new Label
        {
            style =
            {
                color = new StyleColor(new Color(255, 255, 255))
            }
        };
        Add(Label);

        //Add USS style properties to the elements
        _icon.AddToClassList("slotIcon");
        AddToClassList("slotContainer");

        RegisterCallback<PointerDownEvent>(OnPointerDown);
        RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    public void HoldItem(InventoryItem item)
    {
        //_icon.image = item.Icon.texture;
        Label.text = $"{item.item.shortName} - {item.count}";
        Item = item;
    }

    public void DropItem()
    {
        Item = null;
        Label.text = null;
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        if (IsTradeMode) return;
        
        //Not the left mouse button
        if (evt.button != 0 || Item == null)
        {
            return;
        }
        
        //Clear the image
        Label.text = null;

        //Start the drag
        InventoryUIController.StartDrag(evt.position, this);
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (!IsTradeMode) return;

        if (evt.button != 0 || Item == null) return;

        foreach (var cb in TradeCallbacks)
        {
            cb.Invoke(Item);
        }
    }

    #region UXML

    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits>
    {
    }

    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
    }

    #endregion
}