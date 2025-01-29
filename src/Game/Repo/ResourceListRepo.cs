using System.Collections.Generic;
using System.Text.Json;
using Godot;

public interface IResourceListRepo<TData>
{
	List<TData> List();
}

public abstract class ResourceListRepo<TData> : IResourceListRepo<TData>
{
	private static readonly Dictionary<string, List<TData>> _resourceCache = new();

	protected abstract string ResourcePath { get; }
	protected abstract string Key { get; }
	
	public IResourceReader ResourceReader { private get; set; } = new ResourceReader();
	
	public List<TData> List()
	{
		if (_resourceCache.TryGetValue(Key, out var value))
			return value;

		var contents = ResourceReader.Read(ResourcePath);
		if (string.IsNullOrWhiteSpace(contents))
		{
			GD.PrintErr($"Resource contents for path {ResourcePath} was null/whitespace");
		}

		List<TData> data;
		try
		{
			data = JsonSerializer.Deserialize<List<TData>>(contents);
		}
		catch
		{
			GD.PrintErr($"Failed to deserialize resource: {contents}");
			throw;
		}

		_resourceCache[Key] = data;

		return data;
	}
}
