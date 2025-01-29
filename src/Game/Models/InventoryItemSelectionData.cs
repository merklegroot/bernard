using System;

namespace Game.Models;

public enum InventoryItemSelectionSource {
    Invalid = 0,
    Inventory = 1,
    Room = 2
}

public record InventoryItemSelectionData
{
    public InventoryItemSelectionSource Source { get; init; }
    public int Index { get; init; }

    public static implicit operator string(InventoryItemSelectionData data)
    {
        return $"{data.Source}|{data.Index}";
    }

    public static implicit operator InventoryItemSelectionData(string str)
    {
        var parts = str.Split('|');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var index))
        {
            throw new ArgumentException("Invalid format for InventoryItemSelectionData string", nameof(str));
        }

        if (!Enum.TryParse(parts[0], out InventoryItemSelectionSource source))
        {
            throw new ArgumentException("Invalid source for InventoryItemSelectionData string", nameof(str));
        }

        return new InventoryItemSelectionData
        {
            Source = source,
            Index = index
        };
    }
}