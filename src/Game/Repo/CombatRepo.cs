using Game.Models.ViewModels;

namespace Game.Repo;

public interface ICombatRepo
{
    CombatViewModel GetCombatViewModel();
}

public class CombatRepo : ICombatRepo
{
    public CombatViewModel GetCombatViewModel()
    {
        throw new System.NotImplementedException();
    }
}