using SQLite4Unity3d;

namespace Persistence.Models
{
    [Table("settings")]
    public class SettingsItem
    {
        public string key { get; set; }
        
        public ValueType type { get; set; }
        
        public int intValue { get; set; }

        public string stringValue { get; set; }
    }

    public static class Settings
    {
        public const string InventorySize = "INVENTORY_SIZE";
    }
}