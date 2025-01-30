using System;
using Godot;
using System.Collections.Generic;
using Game.Models;
using Game.Repo;

public partial class RoomExitsPanel : Panel
{
    private HFlowContainer _itemContainer;
    private readonly List<Action> _handlers = new();

    private readonly RoomDefRepo _roomDefRepo = new();

    public override void _Ready()
    {
        _itemContainer = GetNode<HFlowContainer>("ItemContainer");
        
        EventBus.Instance.RoomChanged += OnRoomChanged;
        UpdateDisplay();
    }

    private void OnRoomChanged()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var roomDef = _roomDefRepo.Get(GameStateContainer.GameState.RoomId);

        UpdateExits(roomDef.Exits);
    }

    public void UpdateExits(List<RoomExit> exits)
    {
        var children = _itemContainer.GetChildren();
        for (var index = 0; index < children.Count; index++)
        {
            var child = children[index];
            if (child is Button button)
                button.Pressed -= _handlers[index];

            child.QueueFree();
        }
        
        _handlers.Clear();

        if (exits.Count == 0)
        {
            var label = new Label { Text = "None" };
            _itemContainer.AddChild(label);
            return;
        }

        for (var i = 0; i < exits.Count; i++)
        {
            var exit = exits[i];
            var button = new Button
            {
                Text = exit.Direction.ToString(),
                CustomMinimumSize = new Vector2(100, 30),
                SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter
            };
            
            var handler = new Action(() => GD.Print($"Exit clicked: {exit.Direction}"));
            button.Pressed += handler;
            _handlers.Add(handler);
            
            _itemContainer.AddChild(button);
        }
    }
} 