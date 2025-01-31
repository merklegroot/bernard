using System.Collections.Generic;
using System.Linq;
using Game.Constants;
using Game.Models;

namespace Game.Models;

public class GameState
{
	public string RoomId { get; set; }
	public int Health { get; set; }
	public int Gold { get; set; }
	
	public PanelEnum CurrentMainPanel { get; set; }

	public List<InventoryItem> Inventory { get; set; }
}
