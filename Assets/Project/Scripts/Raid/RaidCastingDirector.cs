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

        public List<VenturerBehaviour> VenturerList { get; } = new();
        public VillainBehaviour Villain { get; set; }
        

        public void Initialize()
        {
            var challengers = Camp.Challengers;
            var villain = Den.StageVillain;

            if (!Verify.IsNotNull(challengers) || 
                !Verify.IsTrue(challengers.Count < 7) ||
                !Verify.IsNotDefault(villain)) return;

            SpawnVenturer(challengers);
            SpawnVillain(villain);
        }


        private void SpawnVenturer(IEnumerable<VenturerType> venturerEntry)
        {
            venturerEntry.ForEach((adventurerIndex, index) =>
            {
                if (!Camp.GetVenturerPrefab(adventurerIndex, out var adventurerPrefab)) return;

                var profitPosition   = RaidDirector.StageDirector.GetAdventurerPosition(index).position;
                var adventurer = Instantiate(adventurerPrefab, profitPosition, Quaternion.identity, venturerHierarchy);
                adventurer.SetActive(true);

                if (!adventurer.TryGetComponent(out VenturerBehaviour vb)) return;

                VenturerList.Add(vb);
            });
            
            VenturerList.ForEach(adventurer => adventurer.ForceInitialize());
        }

        private void SpawnVillain(VillainType villainIndex)
        {
            if (!Den.GetVillainPrefab(villainIndex, out var villainPrefab)) return;

            var profitPosition   = RaidDirector.StageDirector.VillainSpawnPosition.position;
            var villainObject = Instantiate(villainPrefab, profitPosition, Quaternion.identity, villainHierarchy);
            
            villainObject.SetActive(true);

            if (!villainObject.TryGetComponent(out VillainBehaviour villainBehaviour)) return;

            Villain = villainBehaviour;
            Villain.ForceInitialize();
            Villain.DeadBehaviour.OnCompleted.Register("DropItems", () => GetReward(villainIndex));
        }
        
        private void GetReward(VillainType villainCode)
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