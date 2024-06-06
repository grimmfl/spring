using System;
using Persistence.Models;
using ValueType = Persistence.Models.ValueType;

namespace Persistence
{
    public class SettingsRepository : RepositoryBase
    {
        public int GetIntValue(string key)
        {
            var item = Query(c => c.Table<SettingsItem>().FirstOrDefault(e => e.key == key));

            if (item.type != ValueType.Int)
                throw new ArgumentException($"{key}");

            return item.intValue;
        }
    }
}