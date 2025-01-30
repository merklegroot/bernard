using Godot;

public partial class RoomExitsPanel : Panel
{
    private HFlowContainer _itemContainer;

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
        foreach (var child in _itemContainer.GetChildren())
        {
            child.QueueFree();
        }

        // Temporary hardcoded exits
        var label = new Label { Text = "None" };
        _itemContainer.AddChild(label);
    }
} 