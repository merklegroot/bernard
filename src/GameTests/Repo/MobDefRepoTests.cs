using System.Text.Json;
using Game.Models;
using Game.Repo;
using Game.Utils;
using Xunit.Abstractions;

namespace GameTests.Repo;

public class MobDefRepoTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void Should_list_mobs()
    {
        var items = new MobDefRepo(new ResourceTestReader())
            .List();
        
        outputHelper.WriteLine(JsonSerializer.Serialize(items));
    }
}