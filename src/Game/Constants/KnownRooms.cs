using System;

namespace Game.Constants;

public static class KnownRooms
{
    public static readonly Guid CastleEntrance = Guid.Parse("2f3e4d5c-6b7a-8c9d-0e1f-2a3b4c5d6e7f");
    public static readonly Guid Barracks = Guid.Parse("6274df43-761e-44ce-a551-e702408d77d5");
    public static readonly Guid StartingRoomId = CastleEntrance;
}