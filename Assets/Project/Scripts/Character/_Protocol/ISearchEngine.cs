using System.Collections.Generic;

namespace Character
{
    public interface ISearchEngine
    {
        List<ICombatTaker> AdventurerList { get; }
        List<ICombatTaker> MonsterList { get; }
    }
}
