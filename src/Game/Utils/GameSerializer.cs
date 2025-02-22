using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game.Converters;
using Tomlyn;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Game.Utils;

public static class GameSerializer
{
    private static readonly ISerializer YamlSerializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
        .Build();

    private static readonly IDeserializer YamlDeserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

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

    public static string SerializeYml<TModel>(TModel value) => 
        new SerializerBuilder()
            .WithTypeConverter(new DirectionYamlConverter())
            .Build().Serialize(value);

    public static TModel DeserializeYml<TModel>(string yml)
    {
        var deserializer = new DeserializerBuilder()
            .WithTypeConverter(new DirectionYamlConverter())
            .Build();
        return deserializer.Deserialize<TModel>(yml);
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