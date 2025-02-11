namespace Game.Utils;

public static class CreatureUtility
{
    public static int GetMaxHp(int constitution)
    {
        const int baseHp = 10;
        const int hpPerCon = 5;

        return hpPerCon * constitution + baseHp;
    }
}