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
		if (string.IsNullOrEmpty(id))
			throw new ArgumentNullException(nameof(id), $"{GetType().Name} -- {nameof(id)} cannot be null or empty.");
		
		var all = List();
		var matchingItem = all.FirstOrDefault(queryItem =>
			string.Equals(queryItem.Id, id, StringComparison.OrdinalIgnoreCase));

		if (matchingItem == null)
			throw new ApplicationException($"{GetType().Name} found no item with id: {id}");
		
		return matchingItem;
	}
}
