using System.Collections.Generic;
using Game.Constants;

namespace Game.Models;

public class GameState
{
	public string RoomId { get; set; }
	public int Gold { get; set; }
	public int Str { get; set; }
	public int Con { get; set; }
	public int CurrentHp { get; set; }
	
	public PanelEnum CurrentMainPanel { get; set; }

	public List<InventoryItem> Inventory { get; set; }
}
