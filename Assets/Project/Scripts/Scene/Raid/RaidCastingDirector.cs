using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Raid
{
    public class RaidCastingDirector : MonoBehaviour
    {
        [SerializeField] private List<AdventurerBehaviour> adventurerList = new();
        [SerializeField] private List<MonsterBehaviour> monsterList = new();

        public List<AdventurerBehaviour> AdventurerList => adventurerList;
        public List<MonsterBehaviour> MonsterList => monsterList;

        private void Awake()
        {
            SetUp();
        }

        public void SetUp()
        {
            GetComponentsInChildren(AdventurerList);
            GetComponentsInChildren(MonsterList);
        }
    }
}