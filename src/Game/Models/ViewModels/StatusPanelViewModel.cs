namespace Game.Models.ViewModels;

public record StatusPanelViewModel
{
    public int Str { get; init; }
    public int Atk { get; init; }
    public int Def { get; init; }
    public int Con { get; init; }
    public int CurrentHp { get; init; }
    public int MaxHp { get; init; }
    public int Gold { get; init; }
}
