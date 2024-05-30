using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;

namespace Persistence
{
    [Table("inventory")]
    public class InventoryItem
    {
        [PrimaryKey, NotNull, Unique]
        public int idItem { get; set; }
        
        [NotNull]
        public int count { get; set; }
        
        [NotNull]
        public int position { get; set; }
        
        [Ignore]
        public Item item { get; set; }
    }
    
    public class InventoryRepository : RepositoryBase
    {
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
    }
}