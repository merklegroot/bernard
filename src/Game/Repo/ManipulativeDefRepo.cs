using Game.Models;
using Game.Repo;

public interface IManipulativeDefRepo : IResourceEntityListRepo<ManipulativeDef>
{
}

public class ManipulativeDefRepo : ResourceEntityListRepo<ManipulativeDef>, IManipulativeDefRepo
{
	protected override string ResourcePath => "res://data/manipulative-defs.json";
	protected override string Key => "manipulative-defs";
}
