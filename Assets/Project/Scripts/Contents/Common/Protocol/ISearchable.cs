using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public interface ISearchable
    {
        List<GameObject> AdventurerList { get; }
        List<GameObject> MonsterList { get; }
        ICombatTaker MainTarget { get; set; }
        ICombatTaker Self { get; }
    }
}
