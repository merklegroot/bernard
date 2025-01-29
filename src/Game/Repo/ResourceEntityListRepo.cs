using System;
using System.Linq;
using Game.Models;

namespace Game.Repo;

public interface IResourceEntityListRepo<TData> : IResourceListRepo<TData>
    where TData : IHasId
{
    TData Get(string id);
}

public abstract class ResourceEntityListRepo<TData> : ResourceListRepo<TData>, IResourceEntityListRepo<TData>
    where TData : IHasId
{
    public TData Get(string id)
    {
        var all = List();
        var matchingItem = all.FirstOrDefault(queryItem =>
            string.Equals(queryItem.Id, id, StringComparison.OrdinalIgnoreCase));

        if (matchingItem == null)
            throw new ApplicationException($"No item with id: {id}");
		
        return matchingItem;
    }
}