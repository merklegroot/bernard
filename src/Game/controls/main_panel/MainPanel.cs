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
        Game.Events.EventBus.Instance.MainPanelChanged += OnMainPanelChanged;
    }
    
    private void OnMainPanelChanged()
    {
        GD.Print("OnMainPanelChanged");
        ShowCurrentPanel();
    }

    private void ShowCurrentPanel()
    {
        var expectedPanelName = CollectionExtensions
            .GetValueOrDefault(
                GameConstants.PanelNameDictionary, 
                GameStateContainer.GameState.CurrentMainPanel, 
                GameConstants.NothingPanelName);
        
        foreach (var child in GetChildren())
        {
            if (child is not Control panel) continue;
            
            var shouldShow = string.Equals(panel.Name, expectedPanelName, StringComparison.OrdinalIgnoreCase);
            panel.Visible = shouldShow;
        }
    }
}
