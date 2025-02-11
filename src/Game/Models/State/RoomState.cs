using System.Collections.Generic;

namespace Game.Models.State;

public class RoomState
{
    public string RoomId { get; set; }
    
    public List<ManipulativeInstance> ManipulativeInstances { get; set; } = new();
}