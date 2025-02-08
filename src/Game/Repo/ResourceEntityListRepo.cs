using System;
using System.Linq;
using Game.Models;

namespace Game.Repo;

public interface IResourceEntityListRepo<TData> : IResourceListRepo<TData>
	where TData : IHasId<Guid>
{
	TData Get(Guid id);
}

public abstract class ResourceEntityListRepo<TData> : ResourceListRepo<TData>, IResourceEntityListRepo<TData>
	where TData : IHasId<Guid>
{
	public TData Get(Guid id)
	{
		if(id == Guid.Empty)
			throw new ArgumentNullException(nameof(id), $"{GetType().Name} -- {nameof(id)} must not be empty.");
		
		var all = List();
		var matchingItem = all.FirstOrDefault(queryItem =>
			queryItem.Id == id);

		if (matchingItem == null)
			throw new ApplicationException($"{GetType().Name} found no item with id: {id}");
		
		return matchingItem;
	}
}
