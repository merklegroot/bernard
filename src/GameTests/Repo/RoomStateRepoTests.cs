using AutoFixture;
using Game.Models;
using Game.Repo;
using NSubstitute;
using Shouldly;

namespace GameTests.Repo;

public class RoomStateRepoTests
{
    private readonly IFixture _fixture = new Fixture();
    
    [Fact]
    public void Should_get_room_state()
    {
        var roomId = _fixture.Create<string>();
        var roomDef = new RoomDef { Id = roomId };

        var roomDefRepo = Substitute.For<IRoomDefRepo>(); 
        roomDefRepo.Get(roomId).Returns(roomDef);

        var roomStateRepo = new RoomStateRepo(roomDefRepo);

        var roomState = roomStateRepo.Get(roomId);

        roomState.ShouldNotBeNull();
        roomState.RoomId.ShouldBe(roomId);
    }

    [Fact]
    public void Should_add_manipulative()
    {
        var roomId = _fixture.Create<string>();
        var roomDef = new RoomDef { Id = roomId };

        var roomDefRepo = Substitute.For<IRoomDefRepo>(); 
        roomDefRepo.Get(roomId).Returns(roomDef);

        var roomStateRepo = new RoomStateRepo(roomDefRepo);
        
        var manipulativeId = _fixture.Create<string>();
        roomStateRepo.AddManipulative(roomId, manipulativeId);

        var retrievedRoom = roomStateRepo.Get(roomId);
        retrievedRoom.ManipulativeIds.Single().ShouldBe(manipulativeId);
    }
}