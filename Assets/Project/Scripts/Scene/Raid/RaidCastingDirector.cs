using System.Collections.Generic;
using Character;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid
{
    public class RaidCastingDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private Transform adventurerHierarchy;
        [SerializeField] private Transform monsterHierarchy;
        [SerializeField] private DataIndex mainBoss;
        
        [SerializeField] private DataIndex firstAdventurer;
        [SerializeField] private DataIndex secondAdventurer;
        [SerializeField] private DataIndex thirdAdventurer;
        [SerializeField] private DataIndex fourthAdventurer;
        
        [SerializeField] private List<GameObject> adventurerPrefabList = new();
        [SerializeField] private List<GameObject> bossPrefabList = new();

        [ShowInInspector] public List<Adventurer> AdventurerList { get; } = new();
        [ShowInInspector] public List<Boss> MonsterList { get; } = new();

        public void Initialize()
        {
            var adventurer1 = adventurerPrefabList.Find(x =>
                x.TryGetComponent(out Adventurer adventurer) && adventurer.ClassCode == firstAdventurer);
            
            var adventurer2 = adventurerPrefabList.Find(x =>
                x.TryGetComponent(out Adventurer adventurer) && adventurer.ClassCode == secondAdventurer);
            
            var adventurer3 = adventurerPrefabList.Find(x =>
                x.TryGetComponent(out Adventurer adventurer) && adventurer.ClassCode == thirdAdventurer);
            
            var adventurer4 = adventurerPrefabList.Find(x =>
                x.TryGetComponent(out Adventurer adventurer) && adventurer.ClassCode == fourthAdventurer);
            
            var boss = bossPrefabList.Find(x =>
                x.TryGetComponent(out Boss boss) && boss.ClassCode == mainBoss);

            var adv1Object = Instantiate(adventurer1, RaidDirector.StageDirector.GetAdventurerPosition(0).position,
                RaidDirector.StageDirector.GetAdventurerPosition(0).rotation, adventurerHierarchy);
            
            var adv2Object = Instantiate(adventurer2, RaidDirector.StageDirector.GetAdventurerPosition(1).position,
                RaidDirector.StageDirector.GetAdventurerPosition(1).rotation, adventurerHierarchy);
            
            var adv3Object = Instantiate(adventurer3, RaidDirector.StageDirector.GetAdventurerPosition(2).position,
                RaidDirector.StageDirector.GetAdventurerPosition(2).rotation, adventurerHierarchy);
            
            var adv4Object = Instantiate(adventurer4, RaidDirector.StageDirector.GetAdventurerPosition(3).position,
                RaidDirector.StageDirector.GetAdventurerPosition(3).rotation, adventurerHierarchy);
            
            var bossObject = Instantiate(boss, RaidDirector.StageDirector.BossSpawn.position,
                RaidDirector.StageDirector.BossSpawn.rotation, monsterHierarchy);
            
            AdventurerList.Add(adv1Object.GetComponent<Adventurer>());
            AdventurerList.Add(adv2Object.GetComponent<Adventurer>());
            AdventurerList.Add(adv3Object.GetComponent<Adventurer>());
            AdventurerList.Add(adv4Object.GetComponent<Adventurer>());
            
            MonsterList.Add(bossObject.GetComponent<Boss>());
        }

        // private void Awake()
        // {
        //     // GetComponentsInChildren(AdventurerList);
        //     // GetComponentsInChildren(MonsterList);
        // }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            // GetComponentsInChildren(AdventurerList);
            // GetComponentsInChildren(MonsterList);
        }
#endif
    }
}