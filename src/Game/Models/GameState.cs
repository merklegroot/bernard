using System.Collections.Generic;

public record GameState
{
	public string RoomId { get; init; }
	public int Health { get; set; }
	public int Gold { get; set; }

	public List<InventoryItem> Inventory { get; set; }
}
