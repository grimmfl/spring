using System.Collections.Generic;
using SQLite4Unity3d;

namespace Persistence.Models
{
    [Table("trader")]
    public class Trader
    {
        public int idTrader { get; set; }
        
        public string name { get; set; }
        
        [Ignore]
        public ICollection<XrefTraderItem> items { get; set; }
    }
}