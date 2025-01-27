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
        
        GD.Print("MainPanel Ready");
    }
    
    public void ShowPanel(Control panel)
    {
        // Hide current panel if it exists
        if (_currentPanel != null)
        {
            _currentPanel.Visible = false;
        }
        
        // Show new panel
        _currentPanel = panel;
        if (_currentPanel != null)
        {
            _currentPanel.Visible = true;
        }
    }
    //
    // public override void _Ready()
    // {
    //     // Hide all panels initially
    //     foreach (var child in GetChildren())
    //     {
    //         if (child is Control panel)
    //         {
    //             panel.Visible = false;
    //         }
    //     }
    //     
    //     // Show the first panel by default (NothingPanel in this case)
    //     var firstPanel = GetChild<Control>(0);
    //     if (firstPanel != null)
    //     {
    //         ShowPanel(firstPanel);
    //     }
    // }

    private void OnMainPanelChanged()
    {
        GD.Print("OnMainPanelChanged");
        ShowCurrentPanel();
    }

    private const string NothingPanelName = "NothingPanel";
    
    private static Dictionary<PanelEnum, string> PanelNameDictionary = new Dictionary<PanelEnum, string>
    {
        { PanelEnum.Nothing, NothingPanelName },
        { PanelEnum.Room, "RoomPanel" },
        { PanelEnum.InventoryDetails, "InventoryDetailsPanel" }
    };
    
    private void ShowCurrentPanel()
    {
        var expectedPanelName = CollectionExtensions.GetValueOrDefault(PanelNameDictionary, GameStateContainer.GameState.MainPanel, NothingPanelName);
        
        foreach (var child in GetChildren())
        {
            if (child is not Control panel) continue;
            
            var shouldShow = string.Equals(panel.Name, expectedPanelName, StringComparison.OrdinalIgnoreCase);
            panel.Visible = shouldShow;
        }
    }
}
