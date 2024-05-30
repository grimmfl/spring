using SQLite4Unity3d;

namespace Persistence.Models
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
}