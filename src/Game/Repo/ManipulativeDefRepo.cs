using System;
using System.Linq;

public class ManipulativeDefRepo : ResourceListRepo<ManipulativeDef>
{
	protected override string ResourcePath => "res://data/manipulative-defs.json";
	protected override string Key => "manipulative-defs";

	public ManipulativeDef Get(string id)
	{
		var all = List();
		var matchingItem = all.FirstOrDefault(queryManipulativeDef =>
			string.Equals(queryManipulativeDef.Id, id, StringComparison.OrdinalIgnoreCase));

		if (matchingItem == null)
			throw new ApplicationException($"No item with id: {id}");
		
		return matchingItem;
	}
}
