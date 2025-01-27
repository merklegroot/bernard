using Game.Models;
using Game.Repo;

public class ManipulativeDefRepo : ResourceEntityListRepo<ManipulativeDef>
{
	protected override string ResourcePath => "res://data/manipulative-defs.json";
	protected override string Key => "manipulative-defs";
}
