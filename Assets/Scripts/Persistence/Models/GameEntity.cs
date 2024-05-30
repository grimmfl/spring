using System.Collections.Generic;
using SQLite4Unity3d;

namespace Persistence.Models
{
    [Table("gameEntity")]
    public class GameEntity
    {
        [PrimaryKey, NotNull, Unique, AutoIncrement]
        public int idGameEntity { get; set; }
        
        [NotNull]
        public string name { get; set; }
        
        public int dismantleSeconds { get; set; }
        
        [NotNull]
        public bool isDismantable { get; set; }
        
        [Ignore]
        public ICollection<XrefGameEntityItem> dropItems { get; set; }
    }
}