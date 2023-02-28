using System.Collections.Generic;
using Character;
using Core;
using UnityEngine;

namespace Raid
{
    public class RaidCastingDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private List<Adventurer> adventurerList = new();
        [SerializeField] private List<Boss> monsterList = new();

        public List<Adventurer> AdventurerList => adventurerList;
        public List<Boss> MonsterList => monsterList;

        private void Awake()
        {
            GetComponentsInChildren(AdventurerList);
            GetComponentsInChildren(MonsterList);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(AdventurerList);
            GetComponentsInChildren(MonsterList);
        }
#endif
    }
}