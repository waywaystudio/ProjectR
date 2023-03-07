using System.Collections.Generic;
using Character;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid
{
    public class RaidCastingDirector : MonoBehaviour
    {
        [SerializeField] private Transform adventurerHierarchy;
        [SerializeField] private Transform monsterHierarchy;

        [ShowInInspector] public List<Adventurer> AdventurerList { get; } = new();
        [ShowInInspector] public List<Boss> MonsterList { get; } = new();


        public void Initialize(List<DataIndex> adventurerEntry, DataIndex bossEntry)
        {
            if (adventurerEntry.IsNullOrEmpty())
            {
                Debug.Log($"Adventurer Not Existed. HasEntry ? : {!adventurerEntry.IsNullOrEmpty()}");
                return;
            }
            
            if (adventurerEntry.Count > 8)
            {
                Debug.Log($"Adventurer Count Error. EntryCount : {adventurerEntry.Count}");
                return;
            }
            
            if (!MainData.CombatClassMaster.Gets(adventurerEntry, out var adventurerPrefabs)) return;
            if (!MainData.BossMaster.Get(bossEntry, out var bossPrefab)) return;

            adventurerPrefabs.ForEach((prefab, index) =>
            {
                var adventurerObject = Instantiate(prefab, 
                    RaidDirector.StageDirector.GetAdventurerPosition(index).position, 
                    Quaternion.identity, 
                    adventurerHierarchy);
                var adventurer = adventurerObject.GetComponent<Adventurer>();
                
                AdventurerList.Add(adventurer);
            });

            var bossObject = Instantiate(bossPrefab, RaidDirector.StageDirector.BossSpawn.position, Quaternion.identity, monsterHierarchy);
            var boss = bossObject.GetComponent<Boss>();
            
            MonsterList.Add(boss);
        }
    }
}