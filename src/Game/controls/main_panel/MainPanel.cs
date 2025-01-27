using System;
using Game.Constants;
using Game.Models;
using Godot;
using Godot.Collections;
using CollectionExtensions = System.Collections.Generic.CollectionExtensions;

public partial class MainPanel : Panel
{
    private Control _currentPanel;

    public override void _Ready()
    {
        ShowCurrentPanel();
        EventBus.Instance.MainPanelChanged += OnMainPanelChanged;
    }
    
    private void OnMainPanelChanged()
    {
        GD.Print("OnMainPanelChanged");
        ShowCurrentPanel();
    }

    private const string NothingPanelName = "NothingPanel";
    
    private static readonly Dictionary<PanelEnum, string> PanelNameDictionary = new()
    {
        { PanelEnum.Nothing, NothingPanelName },
        { PanelEnum.Room, "RoomPanel" },
        { PanelEnum.InventoryDetails, "InventoryDetailsPanel" }
    };
    
    private void ShowCurrentPanel()
    {
        var expectedPanelName = CollectionExtensions.GetValueOrDefault(PanelNameDictionary, GameStateContainer.GameState.CurrentMainPanel, NothingPanelName);
        
        foreach (var child in GetChildren())
        {
            if (child is not Control panel) continue;
            
            var shouldShow = string.Equals(panel.Name, expectedPanelName, StringComparison.OrdinalIgnoreCase);
            panel.Visible = shouldShow;
        }
    }
}
