using System.Collections.Generic;
using System.Linq;
using Persistence.Models;

namespace Persistence
{
    public class ItemRepository : RepositoryBase
    {
        public ICollection<Item> GetAll()
        {
            return Query(c => c.Table<Item>().ToList());
        }
    }
}