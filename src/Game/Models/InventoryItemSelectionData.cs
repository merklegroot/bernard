using System;

namespace Game.Models;

public enum InventoryItemSelectionSource {
	Invalid = 0,
	Inventory = 1,
	Room = 2
}

public record InventoryItemSelectionData(
	InventoryItemSelectionSource Source,
	Guid ManipulativeInstanceId,
	Guid ManipulativeDefId)
{
	public static implicit operator string(InventoryItemSelectionData data)
	{
		return $"{data.Source}|{data.ManipulativeInstanceId}|{data.ManipulativeDefId}";
	}

	public static implicit operator InventoryItemSelectionData(string selectionText)
	{
		var parts = selectionText.Split('|');
		if (parts.Length != 3 
			|| !Guid.TryParse(parts[1], out var instanceId)
			|| !Guid.TryParse(parts[2], out var defId))
		{
			throw new ArgumentException($"Invalid format for InventoryItemSelectionData string: {selectionText}", nameof(selectionText));
		}

		if (!Enum.TryParse(parts[0], out InventoryItemSelectionSource source))
		{
			throw new ArgumentException("Invalid source for InventoryItemSelectionData string", nameof(selectionText));
		}

		return new InventoryItemSelectionData(source, instanceId, defId);
	}
}
