using System;
using System.Collections.Generic;
using Godot;
using System.Text;
using System.Text.Json;
using Game.Models;

public partial class InventoryListPanel : Panel
{
    private readonly ManipulativeDefRepo _manipulativeDefRepo = new();
    private VBoxContainer _container;
    private List<Action> _handlers = new List<Action>();
	
    public override void _Ready()
    {
        _container = GetNode<VBoxContainer>("VBoxContainer");
        
        EventBus.Instance.InventoryChanged += OnInventoryChanged;

        UpdateInventoryItems();
    }

    private void AddInventoryItem(ManipulativeInstance item, int inventoryItemIndex)
    {
        var matchingManipulativeDef = _manipulativeDefRepo.Get(item.Id);
        var itemText = GetDisplayText(matchingManipulativeDef);
		
        var button = new Button
        {
            Text = itemText,
            LayoutMode = 2,
            Alignment = HorizontalAlignment.Left
        };
		
        var handler = new Action(() =>
        {
            var selectionData = (string)new InventoryItemSelectionData
                { Source = InventoryItemSelectionSource.Inventory, Index = inventoryItemIndex }; 
            
            EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryItemSelctedFlexible, selectionData);
        });
		
        button.Pressed += handler;
        _handlers.Add(handler);
		
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
        var children = _container.GetChildren();
        for (var index = 0; index < children.Count; index++)
        {
            var child = children[index];
            if (child is not Button button)
                continue;
            
            button.Pressed -= _handlers[index];
            button.QueueFree();
        }
        
        _handlers.Clear();
        
        for (var index = 0; index < GameStateContainer.GameState.Inventory.Count; index++)
        {
            var item = GameStateContainer.GameState.Inventory[index];
            AddInventoryItem(item, index);
        }
    }
} 