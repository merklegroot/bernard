using System;
using System.Collections.Generic;
using Godot;
using Game.Models;

public partial class InventoryListPanel : Panel
{
    private VBoxContainer _container;
    private List<Action> _handlers = new();
    
    private readonly Texture2D _gearTexture = GD.Load<Texture2D>("res://assets/Pixel Art Icon Pack - RPG/Misc/Gear.png");
	
    public override void _Ready()
    {
        _container = GetNode<VBoxContainer>("VBoxContainer");
        
        EventBus.Instance.InventoryChanged += OnInventoryChanged;

        UpdateInventoryItems();
    }

    private void AddInventoryItem(ManipulativeInstance item, int inventoryItemIndex)
    {
        var button = new ManipulativeButton
        {
            ManipulativeId = item.ManipulativeId,
        };
		
        var handler = new Action(() =>
        {
            var selectionData = (string)new InventoryItemSelectionData
                { Source = InventoryItemSelectionSource.Inventory, Index = inventoryItemIndex }; 
            
            EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryItemSelectedFlexible, selectionData);
        });
		
        button.Pressed += handler;
        _handlers.Add(handler);
		
        _container.AddChild(button);
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