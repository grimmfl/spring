using SQLite4Unity3d;

namespace Persistence.Models
{
    [Table("item")]
    public class Item
    {
        [PrimaryKey, NotNull, AutoIncrement, Unique]
        public int idItem { get; set; }
        
        [NotNull]
        public string name { get; set; }
        
        [NotNull]
        public string shortName { get; set; }
    }
}