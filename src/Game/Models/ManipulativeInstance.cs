using System;

namespace Game.Models;

public record ManipulativeInstance
{
    public Guid Id { get; init; }
    public Guid ManipulativeDefId { get; init; }
};