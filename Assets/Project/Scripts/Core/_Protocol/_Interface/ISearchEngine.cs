using System.Collections.Generic;

namespace Core
{
    public interface ISearchEngine
    {
        List<ICombatTaker> AdventurerList { get; }
        List<ICombatTaker> MonsterList { get; }
    }
}