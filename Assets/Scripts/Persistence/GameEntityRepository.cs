using System.Collections.Generic;
using System.Linq;
using Persistence.Models;

namespace Persistence
{
    public class GameEntityRepository : RepositoryBase
    {
        public GameEntity GetById(int id)
        {
            var result = Query(c => c.Table<GameEntity>().FirstOrDefault(e => e.idGameEntity == id));
            
            LoadDropItems(new [] { result });

            return result;
        }

        private void LoadDropItems(IEnumerable<GameEntity> entities)
        {
            var entityList = entities.ToList();

            var ids = entityList.Select(e => e.idGameEntity).Distinct();

            var result = Query(c => c.Table<XrefGameEntityItem>().Where(e => ids.Contains(e.idGameEntity)).ToList());

            var lookup = result.ToLookup(x => x.idGameEntity);

            foreach (var entity in entityList)
            {
                entity.dropItems = lookup[entity.idGameEntity].ToList();
            }
        }
    }
}