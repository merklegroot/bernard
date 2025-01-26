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
}
