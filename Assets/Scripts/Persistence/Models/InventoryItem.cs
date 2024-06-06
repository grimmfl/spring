using System;
using SQLite4Unity3d;

namespace Persistence.Models
{
    [Table("inventory")]
    public class InventoryItem : IEquatable<InventoryItem>
    {
        [PrimaryKey, NotNull, Unique]
        public int idItem { get; set; }
        
        [NotNull]
        public int count { get; set; }
        
        [NotNull]
        public int position { get; set; }
        
        [Ignore]
        public Item item { get; set; }

        public bool Equals(InventoryItem other)
        {
            if (other == null) return false;

            return other.idItem == idItem;
        }
    }
}