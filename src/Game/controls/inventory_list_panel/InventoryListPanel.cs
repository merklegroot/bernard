using System.Collections.Generic;
using Godot;
using System.Text;
using System.Text.Json;
using Game.Models;

public partial class InventoryListPanel : Panel
{
    private readonly ManipulativeDefRepo _manipulativeDefRepo = new();
    private VBoxContainer _container;
	
    public override void _Ready()
    {
        _container = GetNode<VBoxContainer>("VBoxContainer");
        
        EventBus.Instance.InventoryChanged += OnInventoryChanged;

        UpdateInventoryItems();
    }

    private void AddInventoryItem(InventoryItem item, int inventoryItemIndex)
    {
        var matchingManipulativeDef = _manipulativeDefRepo.Get(item.Id);
        var itemText = GetDisplayText(matchingManipulativeDef);
		
        var button = new Button
        {
            Text = itemText,
            LayoutMode = 2,
            Alignment = HorizontalAlignment.Left
        };
		
        button.Pressed += () =>
        {
            EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryItemSelected, inventoryItemIndex);
        };
		
        _container.AddChild(button);
    }
	
    private string GetDisplayText(ManipulativeDef manipulativeDef)
    {
        var displayTextBuilder = new StringBuilder()
            .AppendLine($" - {manipulativeDef.Name}");

        displayTextBuilder
            .AppendLine(manipulativeDef.IsWeapon ? " (Weapon)" : " (Misc)");

        return displayTextBuilder.ToString();
    }

    private void OnInventoryChanged()
    {
        UpdateInventoryItems();
    }

    private void UpdateInventoryItems()
    {
        var items = GameStateContainer.GameState.Inventory;

        foreach (var child in _container.GetChildren())
        {
            child.QueueFree();
        }

        var index = 0;
        foreach (var item in items)
        {
            AddInventoryItem(item, index);
            index++;
        }
    }
} 