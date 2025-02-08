using System.Collections.Generic;
using Game.Models;

public class RoomState
{
    public string RoomId { get; set; }
    
    public List<ManipulativeInstance> ManipulativeInstances { get; set; } = new();
}
