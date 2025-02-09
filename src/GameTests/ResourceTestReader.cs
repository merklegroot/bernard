namespace GameTests;

public class TestResourceReader : IResourceReader
{
    public string Read(string resourcePath)
    {
        // "res://data/manipulative-defs.json";
        var filePath = ResToFilePath(resourcePath);
        
        return File.ReadAllText(filePath);
    }

    public void Write(string resourcePath, string contents)
    {
        var filePath = ResToFilePath(resourcePath);
        File.WriteAllText(filePath, contents);
    }

    private string ResToFilePath(string resourcePath)
    {
        const string resPortion = "res://";
        if (!resourcePath.StartsWith(resPortion))
        {
            throw new ArgumentException($"Resource path must start with {resPortion}", nameof(resourcePath));
        }

        var afterRes = resourcePath.Substring(resPortion.Length);
        
        // Get the test project directory and navigate up to find Game project
        var testProjectDir = AppDomain.CurrentDomain.BaseDirectory;
        var solutionDir = Directory.GetParent(testProjectDir)?.Parent?.Parent?.Parent?.Parent?.FullName;
        
        if (solutionDir == null)
        {
            throw new InvalidOperationException("Could not locate solution directory from test directory");
        }

        var gameProjectDir = Path.Combine(solutionDir, "Game");
        if (!Directory.Exists(gameProjectDir))
        {
            throw new InvalidOperationException($"Game project directory not found at {gameProjectDir}");
        }

        return Path.Combine(gameProjectDir, afterRes);
    }
}
