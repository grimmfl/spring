using System;
using Persistence.Models;

namespace Persistence
{
    public class SettingsRepository : RepositoryBase
    {
        public int GetIntValue(string key)
        {
            var item = Query(c => c.Table<SettingsItem>().FirstOrDefault(e => e.key == key));

            if (item.type != SettingsType.Int)
                throw new ArgumentException($"{key}");

            return item.intValue;
        }
    }
}