namespace Game.Utils;

public static class StatCalculator
{
    private const int BASE_HP = 10;
    private const int HP_PER_CON = 5;
    
    public static int CalculateMaxHp(int constitution)
    {
        return BASE_HP + (constitution * HP_PER_CON);
    }
} 