using System.Collections.Generic;
using Common.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid
{
    public class RaidCastingDirector : MonoBehaviour
    {
        [SerializeField] private RaidDirector raidDirector;
        [SerializeField] private List<AdventurerBehaviour> adventurerList = new();
        [SerializeField] private List<MonsterBehaviour> monsterList = new();

        public List<AdventurerBehaviour> AdventurerList => adventurerList;
        public List<MonsterBehaviour> MonsterList => monsterList;
        // public Dictionary<int, AdventurerBehaviour> AdventurerTable { get; } = new();
        // public Dictionary<int, MonsterBehaviour> MonsterTable { get; } = new();

        // public void SpawnCharacter()
        // private void SpawnAdventurer()
        // private void SpawnMonster()

        private void Awake()
        {
            raidDirector ??= GetComponentInParent<RaidDirector>();
            raidDirector.AdventurerList = AdventurerList;
            raidDirector.MonsterList = MonsterList;
            
            // AdventurerList.ForEach(x => AdventurerTable.Add(x.ID, x));
            // MonsterList.ForEach(x => MonsterTable.Add(x.ID, x));
        }

        private void GetList()
        {
            AdventurerList.AddRange(GetComponentsInChildren<AdventurerBehaviour>());
            MonsterList.AddRange(GetComponentsInChildren<MonsterBehaviour>());
        }

#if UNITY_EDITOR
        private void SetUp()
        {
            raidDirector = GetComponentInParent<RaidDirector>();
        }
#endif
    }
}