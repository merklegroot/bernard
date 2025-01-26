using Shouldly;

namespace GameTests.Repo;

public class ManipulativeDefRepoTests
{
    [Fact]
    public void Should_read_manipulated_defs()
    {
        var repo = new ManipulativeDefRepo
        {
            ResourceReader = new TestResourceReader()
        };

        var all = repo.List();
        
        all.ShouldNotBeEmpty();
    }
}