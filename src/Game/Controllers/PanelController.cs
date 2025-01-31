using Game.Constants;
using Game.Models;

namespace Game.Controllers;

// ReSharper disable once UnusedType.Global
public class PanelController : IController
{
    public void Register()
    {
        EventBus.Instance.SetMainPanel += OnSetMainPanel;
    }
    
    private void OnSetMainPanel(int panelId)
    {
        var panelEnum = (PanelEnum)panelId;
        GameStateContainer.GameState.CurrentMainPanel = panelEnum;
        EventBus.Instance.EmitSignal(EventBus.SignalName.MainPanelChanged);
    }
}