using GoogleSheet.Core.Type;

namespace Adventurer
{
    [UGS(typeof(Job))]
    public enum Job
    {
        None = 0,
        Warrior,
        Paladin,
        Load,
        DeathKnight,
        Druid,
        Rogue,
        Lancer,
        Fighter,
        Berserker,
        Blade,
        Hunter,
        Wizard,
        Warlock,
        Sniper,
        Summoner,
        Priest,
        Angel,
        Bard,
        Shaman,
        HolyKnight,
        End = int.MaxValue
    }
}
