using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;

namespace Persistence
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
    
    public class ItemRepository : RepositoryBase
    {
        public ICollection<Item> GetAll()
        {
            return Query(c => c.Table<Item>().ToList());
        }
    }
}