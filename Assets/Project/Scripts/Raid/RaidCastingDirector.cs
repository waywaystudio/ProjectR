using System.Collections.Generic;
using Adventurers;
using Character.Adventurers;
using Character.Bosses;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid
{
    public class RaidCastingDirector : MonoBehaviour
    {
        [SerializeField] private Transform adventurerHierarchy;
        [SerializeField] private Transform monsterHierarchy;
        [SerializeField] private Boss boss;

        [ShowInInspector] public List<Adventurer> AdventurerList { get; } = new();
        [ShowInInspector] public Boss Boss => boss;


        public void Initialize(List<DataIndex> adventurerEntry)
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

            adventurerEntry.ForEach((adventurerIndex, index) =>
            {
                if (!Database.CombatClassMaster.Get<Adventurer>(adventurerIndex, out var adventurerPrefab)) return;
                
                var profitPosition   = RaidDirector.StageDirector.GetAdventurerPosition(index).position;
                var adventurer = Instantiate(adventurerPrefab, profitPosition, Quaternion.identity,
                    adventurerHierarchy);

                adventurer.gameObject.SetActive(true);
                AdventurerList.Add(adventurer);
            });
        }
    }
}