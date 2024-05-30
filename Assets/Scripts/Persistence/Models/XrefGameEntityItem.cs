using System.Collections.Generic;
using SQLite4Unity3d;

namespace Persistence.Models
{
    [Table("gameEntity_item")]
    public class XrefGameEntityItem
    {
        public int idGameEntity { get; set; }
        
        public int idItem { get; set; }
        
        public int min { get; set; }
        
        public int max { get; set; }
    }
}