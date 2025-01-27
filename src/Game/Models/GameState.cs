using System.Collections.Generic;
using System.Linq;
using Game.Constants;

public class GameState
{
	public string RoomId { get; set; }
	public int Health { get; set; }
	public int Gold { get; set; }
	
	public Queue<PanelEnum> MainPanelStack { get; set; }

	public PanelEnum CurrentMainPanel
	{
		get =>
			MainPanelStack.Any() ? MainPanelStack.Peek() : PanelEnum.Nothing;
	}

	public List<InventoryItem> Inventory { get; set; }
}
