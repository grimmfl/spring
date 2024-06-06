using SQLite4Unity3d;

namespace Persistence.Models
{
    [Table("trader_item")]
    public class XrefTraderItem
    {
        public int idTrader { get; set; }
        
        public int idItem { get; set; }
        
        public int price { get; set; }
        
        [Ignore]
        public Item item { get; set; }
    }
}