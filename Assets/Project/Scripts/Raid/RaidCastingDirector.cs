using System.Collections.Generic;
using Adventurers;
using Character.Adventurers;
using Common;
using Monsters;
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
                if (!MainAdventurer.TryGetAdventurer(adventurerIndex, out var adventurerObject)) return;
                
                adventurerObject.transform.SetParent(adventurerHierarchy);
                adventurerObject.transform.position = RaidDirector.StageDirector.GetAdventurerPosition(index).position;
                adventurerObject.SetActive(true);
                
                var adventurer = adventurerObject.GetComponent<Adventurer>();
                
                AdventurerList.Add(adventurer);
            });
        }
    }
}