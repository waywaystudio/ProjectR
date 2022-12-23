using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public interface ISearchable
    {
        List<GameObject> AdventureList { get; }
        List<GameObject> MonsterList { get; }
        ICombatTaker MainTarget { get; set; }
    }
}
