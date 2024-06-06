using SQLite4Unity3d;

namespace Persistence.Models
{
    [Table("stat")]
    public class Stat
    {
        [PrimaryKey]
        public Stats idStat { get; set; }
        
        public ValueType type { get; set; }
        
        public int? intValue { get; set; }
    }
}