using Godot;
using System.Collections.Generic;
using Game.Models;
using Game.Repo;

public partial class RoomExitsPanel : Panel
{
    private HFlowContainer _itemContainer;

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

    private void UpdateExits(List<RoomExit> exits)
    {
        foreach (var child in _itemContainer.GetChildren())
        {
            child.QueueFree();
        }

        if (exits.Count == 0)
        {
            var label = new Label { Text = "None" };
            _itemContainer.AddChild(label);
            return;
        }

        foreach (var exit in exits)
        {
            var button = new Button
            {
                Text = exit.Direction.ToString(),
                CustomMinimumSize = new Vector2(100, 30),
                SizeFlagsHorizontal = SizeFlags.ShrinkCenter
            };
            
            _itemContainer.AddChild(button);
        }
    }
} 