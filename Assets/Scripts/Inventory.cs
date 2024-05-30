using System.Collections.Generic;
using System.Linq;
using Persistence;
using Persistence.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public UIDocument ui;
    public int columnCount;

    private bool _initialized;
    private IList<VisualElement> _columns;

    private ICollection<InventoryItem> _inventory;
    
    private InventoryRepository _inventoryRepository;
    
    // Start is called before the first frame update
    void Start()
    {
        _inventoryRepository = new InventoryRepository();
        _inventory = _inventoryRepository.GetInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            _inventory = _inventoryRepository.GetInventory();
            
            ui.gameObject.SetActive(!ui.gameObject.activeInHierarchy);

            if (ui.gameObject.activeInHierarchy)
            {
                var rootElement = ui.rootVisualElement;
            
                _columns = new int[columnCount]
                    .Select((_, idx) => rootElement.Q<VisualElement>($"InvColumn{idx + 1}"))
                    .ToList();
                
                UpdateInventoryUI();
            }
        }
    }

    private void UpdateInventoryUI()
    {
        var positionToItem = _inventory.ToDictionary(i => i.position, i => i);
        
        for (var i = 0; i < _columns.Count; i++)
        {
            var col = _columns[i];
            
            col.Clear();
            
            if (!positionToItem.TryGetValue(i, out var item)) continue;
            
            col.Add(new Label
            {
                text = $"{item.item.shortName} - {item.count}"
            });
        }
    }
}
