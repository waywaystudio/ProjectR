using System.Collections.Generic;
using Character;
using Core;
using UnityEngine;

namespace Raid
{
    public class RaidCastingDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private List<AdventurerBehaviour> adventurerList = new();
        [SerializeField] private List<MonsterBehaviour> monsterList = new();

        public List<AdventurerBehaviour> AdventurerList => adventurerList;
        public List<MonsterBehaviour> MonsterList => monsterList;

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