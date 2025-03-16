using System;
using System.IO;
using System.Linq;
using System.Reflection;

public interface IResourceReader
{
    string Read(string resourcePath);
}

public class ResourceReader : IResourceReader
{
    public string Read(string resourcePath)
    {
        return Godot.FileAccess.GetFileAsString(resourcePath);
    }

    public string ReadEmbedded(string resourcePath)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var names = assembly.GetManifestResourceNames();
        var matchingName = names.First(queryName => queryName.EndsWith(resourcePath, StringComparison.OrdinalIgnoreCase));
        using var stream = assembly.GetManifestResourceStream(matchingName)!;
        using var streamReader = new StreamReader(stream);
        var contents = streamReader.ReadToEnd();
        
        return contents;
    }
}
