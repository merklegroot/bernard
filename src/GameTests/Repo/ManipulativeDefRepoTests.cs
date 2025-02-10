using Shouldly;

namespace GameTests.Repo;

public class ManipulativeDefRepoTests
{
    [Fact]
    public void Should_read_manipulated_defs()
    {
        var repo = new ManipulativeDefRepo
        {
            ResourceReader = new ResourceTestReader()
        };

        var all = repo.List();
        
        all.ShouldNotBeEmpty();
    }
}