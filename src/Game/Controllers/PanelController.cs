using Game.Constants;
using Game.Models;
using Game.Models.State;

namespace Game.Controllers;

// ReSharper disable once UnusedType.Global
public class PanelController : IController
{
    public void Register()
    {
        Events.EventBus.Instance.SetMainPanel += OnSetMainPanel;
    }
    
    private void OnSetMainPanel(int panelId)
    {
        var panelEnum = (PanelEnum)panelId;
        GameStateContainer.GameState.CurrentMainPanel = panelEnum;
        Events.EventBus.Instance.EmitSignal(Events.EventBus.SignalName.MainPanelChanged);
    }
}