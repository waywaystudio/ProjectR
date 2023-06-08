using System.Collections.Generic;
using System.Text;
using Character.Venturers;
using Character.Villains;
using Common;
using UnityEngine;

namespace Raid
{
    public class RaidCastingDirector : MonoBehaviour
    {
        [SerializeField] private Transform venturerHierarchy;
        [SerializeField] private Transform villainHierarchy;
        [SerializeField] private DataIndex villainCode;

        public List<VenturerBehaviour> VenturerList { get; } = new();
        public VillainBehaviour Villain { get; set; }
        

        public void Initialize(List<DataIndex> venturerEntry)
        {
            if (venturerEntry.IsNullOrEmpty())
            {
                Debug.Log($"Adventurer Not Existed. HasEntry ? : {!venturerEntry.IsNullOrEmpty()}");
                return;
            }
            
            if (venturerEntry.Count > 6)
            {
                Debug.Log($"Adventurer Count Error. EntryCount : {venturerEntry.Count}");
                return;
            }

            SpawnVenturer(venturerEntry);
            SpawnVillain(villainCode);
        }


        private void SpawnVenturer(IEnumerable<DataIndex> venturerEntry)
        {
            venturerEntry.ForEach((adventurerIndex, index) =>
            {
                if (!Database.CombatClassPrefabData.Get<VenturerBehaviour>(adventurerIndex, out var adventurerPrefab)) return;
                
                var profitPosition   = RaidDirector.StageDirector.GetAdventurerPosition(index).position;
                var adventurer = Instantiate(adventurerPrefab, profitPosition, Quaternion.identity,
                                             venturerHierarchy);

                adventurer.gameObject.SetActive(true);
                VenturerList.Add(adventurer);
            });
            
            VenturerList.ForEach(adventurer => adventurer.ForceInitialize());
        }

        private void SpawnVillain(DataIndex villainCode)
        {
            if (!Database.BossPrefabData.GetObject(villainCode, out var villainPrefab)) return;

            var profitPosition   = RaidDirector.StageDirector.VillainSpawnPosition.position;
            var villainObject = Instantiate(villainPrefab, profitPosition, Quaternion.identity, villainHierarchy);
            var villainData = Den.GetVillainData(villainCode);
            
            villainObject.SetActive(true);

            if (!villainObject.TryGetComponent(out VillainBehaviour villainBehaviour)) return;

            Villain = villainBehaviour;
            Villain.ForceInitialize();
            Villain.DeadBehaviour.OnCompleted.Register("DropItems", () => GetReward(villainCode));
        }
        
        private void GetReward(DataIndex villainCode)
        {
            var data = Den.GetVillainData(villainCode);
            
            var ethosType = data.RepresentEthos;
            var difficulty = data.Difficulty;
            var isFirstDefeatTry = data.KillCount == 0;
            var shardOfViciousCount = 0;
            var stoneOfViciousCount = 0;
            var crystalOfPathfinderCount = 50;
            var clearProgress = Random.Range(1, 2);                    // Den에서 받기.
            var rewardMultiplier = Random.Range(1, 2) * clearProgress; // clearProgress로부터 계산.

            if (ethosType.IsVirtue())
            {
                shardOfViciousCount = 2 ;
                stoneOfViciousCount = 2 ;
            }
            else if (ethosType.IsDeficiency())
            {
                shardOfViciousCount = 3 ;
                stoneOfViciousCount = 1 ;
            }
            else if (ethosType.IsExcess())
            {
                shardOfViciousCount = 1 ;
                stoneOfViciousCount = 3 ;
            }
            
            shardOfViciousCount      *= rewardMultiplier;
            stoneOfViciousCount      *= rewardMultiplier;
            crystalOfPathfinderCount *= rewardMultiplier;
            
            var sb = new StringBuilder();
            
            // 경험치만.
            if (!isFirstDefeatTry)
            {
                Camp.AddGrowMaterial(GrowMaterialType.CrystalOfPathfinder, crystalOfPathfinderCount);
                sb.Append($"{GrowMaterialType.CrystalOfPathfinder} {crystalOfPathfinderCount} Get!\n");
                return;
            }
            
            Camp.AddGrowMaterial(GrowMaterialType.ShardOfVicious, shardOfViciousCount);
            Camp.AddGrowMaterial(GrowMaterialType.StoneOfVicious , stoneOfViciousCount);
            Camp.AddGrowMaterial(GrowMaterialType.CrystalOfPathfinder, crystalOfPathfinderCount);

            sb.Append($"{GrowMaterialType.ShardOfVicious} {shardOfViciousCount} Get!\n");
            sb.Append($"{GrowMaterialType.StoneOfVicious} {stoneOfViciousCount} Get!\n");
            sb.Append($"{GrowMaterialType.CrystalOfPathfinder} {crystalOfPathfinderCount} Get!\n");
            
            Debug.Log(sb.ToString());
        }
    }
}