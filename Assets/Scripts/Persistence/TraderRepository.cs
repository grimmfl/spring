using System.Collections.Generic;
using System.Linq;
using Persistence.Models;

namespace Persistence
{
    public class TraderRepository : RepositoryBase
    {
        public Trader GetById(int id)
        {
            var result = Query(c => c.Table<Trader>().FirstOrDefault(e => e.idTrader == id));
            
            LoadItemXrefs(new [] { result });

            return result;
        }

        private void LoadItemXrefs(IEnumerable<Trader> entities)
        {
            var entityList = entities.ToList();

            var ids = entityList.Select(e => e.idTrader).Distinct();

            var result = Query(c => c.Table<XrefTraderItem>().Where(e => ids.Contains(e.idTrader)).ToList());

            LoadItems(result);
            
            var lookup = result.ToLookup(x => x.idTrader);

            foreach (var entity in entityList)
            {
                entity.items = lookup[entity.idTrader].ToList();
            }
        }

        private void LoadItems(List<XrefTraderItem> xrefs)
        {
            var idsItem = xrefs.Select(x => x.idItem).Distinct();

            var result = Query(c => c.Table<Item>().Where(e => idsItem.Contains(e.idItem)).ToList());

            var idToItem = result.ToDictionary(i => i.idItem);

            foreach (var xref in xrefs)
            {
                xref.item = idToItem[xref.idItem];
            }
        }
    }
}