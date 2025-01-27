using System.Collections.Generic;
using Game.Constants;

public class GameState
{
	public string RoomId { get; set; }
	public int Health { get; set; }
	public int Gold { get; set; }
	
	public PanelEnum MainPanel { get; set; }

	public List<InventoryItem> Inventory { get; set; }
}
