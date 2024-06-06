using System;
using System.Collections.Generic;
using System.Linq;
using Persistence.Models;
using ValueType = Persistence.Models.ValueType;

namespace Persistence
{
    public class StatRepository : RepositoryBase
    {
        public static readonly IDictionary<Stats, ICollection<Action<int>>> OnIntValueChanged = new Dictionary<Stats, ICollection<Action<int>>>
        {
            { Stats.Money, new List<Action<int>>() }
        };

        public StatRepository()
        {
            InvokeOnIntValueChanged();
        }
        
        public int GetIntValue(Stats id)
        {
            var item = Query(c => c.Table<Stat>().FirstOrDefault(e => e.idStat == id));

            if (item.type != ValueType.Int)
                throw new ArgumentException($"{id}");

            return item.intValue!.Value;
        }

        public IDictionary<Stats, int> GetIntValues(IEnumerable<Stats> ids)
        {
            var values = Query(c => c.Table<Stat>().Where(e => ids.Contains(e.idStat)).ToList());
            
            if (values.Any(v => v.type != ValueType.Int))
                throw new ArgumentException($"{ids}");

            return values.ToDictionary(v => v.idStat, v => v.intValue!.Value);
        }

        public void SetIntValue(Stats id, int value)
        {
            var stat = Query(c => c.Table<Stat>().FirstOrDefault(e => e.idStat == id));
            
            if (stat.type != ValueType.Int)
                throw new ArgumentException($"{id}");

            stat.intValue = value;
            
            Execute(c => c.Update(stat));
            
            InvokeOnIntValueChanged();
        }

        private void InvokeOnIntValueChanged()
        {
            var values = GetIntValues(OnIntValueChanged.Keys);
            
            foreach (var (stat, callbacks) in OnIntValueChanged)
            {
                var value = values[stat];

                foreach (var cb in callbacks)
                {
                    cb.Invoke(value);
                }
            }
        }
    }
}