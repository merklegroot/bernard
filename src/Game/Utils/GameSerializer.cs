using System.Collections;
using System.Collections.Generic;
using Tomlyn;

namespace Game.Utils;

public static class GameSerializer
{
    private record TomlRootCollectionContainer<TCollection>
    {
        public TCollection TomlRootItems { get; init; }
    }
    
    public static TModel DeserializeToml<TModel>(string contents)
        where TModel : class, new()
    {
        if (!IsArrayLike<TModel>())
        {
            return Toml.ToModel<TModel>(contents);
        }
        
        var withContainer = Toml.ToModel<TomlRootCollectionContainer<TModel>>(contents);
        
        return withContainer.TomlRootItems;
    }

    public static string SerializeToml<TModel>(TModel model)
    {
        if (!IsArrayLike<TModel>())
        {
            return Toml.FromModel(model);    
        }

        return Toml.FromModel(new TomlRootCollectionContainer<TModel>
        {
            TomlRootItems = model
        });
    }

    private static bool IsArrayLike<T>()
    {
        var type = typeof(T);
        
        return
            type.IsArray ||
            typeof(ICollection<>).IsAssignableFrom(type) ||
            typeof(ICollection).IsAssignableFrom(type) ||
            typeof(IList<>).IsAssignableFrom(type) ||
            typeof(IList).IsAssignableFrom(type) ||
            typeof(IEnumerable<>).IsAssignableFrom(type) ||
            typeof(IEnumerable).IsAssignableFrom(type);
    }
}