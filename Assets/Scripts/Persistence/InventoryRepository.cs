using System;
using System.Collections.Generic;
using System.Linq;
using Persistence.Models;

namespace Persistence
{
    public class InventoryRepository : RepositoryBase
    {
        private readonly SettingsRepository _settingsRepository = new SettingsRepository();

        public static readonly ICollection<Action<ICollection<InventoryItem>>> OnInventoryChanged =
            new List<Action<ICollection<InventoryItem>>>();

        public InventoryRepository()
        {
            InvokeOnInventoryChangedCallbacks();
        }
        
        public ICollection<InventoryItem> GetInventory()
        {
            var inventory = Query(c => c.Table<InventoryItem>().ToList());

            var idsItem = inventory.Select(i => i.idItem).Distinct();

            var items = Query(c => c.Table<Item>().Where(e => idsItem.Contains(e.idItem)).ToList());

            var itemDict = items.ToDictionary(i => i.idItem);

            foreach (var item in inventory)
            {
                item.item = itemDict[item.idItem];
            }

            return inventory;
        }

        /// <summary>
        /// Add items to the player's inventory
        /// </summary>
        /// <param name="idsToCount">dictionary mapping from item id to count</param>
        /// <returns>the same dictionary with items that could not fit into the inventory</returns>
        public IDictionary<int, int> AddItems(IDictionary<int, int> idsToCount)
        {
            var ids = idsToCount.Keys;
            
            var toUpdate = Query(c => c
                .Table<InventoryItem>()
                .Where(e => ids.Contains(e.idItem))
                .ToList());

            var idsToCreate = idsToCount
                .Where(pair => toUpdate.All(i => i.idItem != pair.Key))
                .ToList();

            var freePositions = GetFreePositions(idsToCreate.Count).ToList();
            
            var toCreate = idsToCreate.Take(freePositions.Count).Select((pair, idx) => new InventoryItem
                {
                    idItem = pair.Key,
                    count = pair.Value,
                    position = freePositions[idx]
                });
            
            Execute(c =>
            {
                c.InsertAll(toCreate);
                
                foreach (var item in toUpdate)
                {
                    item.count += idsToCount[item.idItem];
                }

                c.UpdateAll(toUpdate);
            });
            
            InvokeOnInventoryChangedCallbacks();
            
            return idsToCreate
                .Skip(freePositions.Count)
                .ToDictionary(p => p.Key, p => p.Value);
        }

        private ICollection<int> GetFreePositions(int count)
        {
            var size = _settingsRepository.GetIntValue(Settings.InventorySize);
            
            var loadedPositions = Query(c => c.Table<InventoryItem>().Select(e => e.position).ToList());

            var freePositions = new bool[size].Select((_, idx) => idx).Except(loadedPositions);

            return freePositions.Take(count).ToList();
        }

        public void RemoveItems(IDictionary<int, int> idsToCount)
        {
            var ids = idsToCount.Keys;

            var items = Query(c => c.Table<InventoryItem>().Where(i => ids.Contains(i.idItem)).ToList());

            var toDelete = items.Where(i => i.count - idsToCount[i.idItem] <= 0).ToList();

            var toUpdate = items.Except(toDelete).ToList();
            
            Execute(c =>
            {
                foreach (var item in toDelete)
                {
                    c.Delete<InventoryItem>(item.idItem);
                }

                foreach (var item in toUpdate)
                {
                    item.count -= idsToCount[item.idItem];
                }

                c.UpdateAll(toUpdate);
            });
            
            InvokeOnInventoryChangedCallbacks();
        }

        private void InvokeOnInventoryChangedCallbacks()
        {
            var inventory = GetInventory();

            foreach (var cb in OnInventoryChanged)
            {
                cb.Invoke(inventory);
            }
        }
    }
}