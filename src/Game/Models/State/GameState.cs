using Game.Constants;

namespace Game.Models.State;

public class GameState
{
	public PanelEnum CurrentMainPanel { get; set; }
	
	public CombatState CombatState { get; set; }
	
	public PlayerState PlayerState { get; set; }
}
