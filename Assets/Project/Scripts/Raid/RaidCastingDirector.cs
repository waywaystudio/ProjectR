using System.Collections.Generic;
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

            // TODO.TEMP TEST
            for (var i = 0; i < 3; i++)
            {
                SpawnVillain(villain);
            }
        }


        private void SpawnVenturer(IEnumerable<VenturerType> venturerEntry)
        {
            venturerEntry.ForEach((adventurerIndex, index) =>
            {
                if (adventurerIndex == VenturerType.None) return;

                var profitPosition   = RaidDirector.StageDirector.GetAdventurerPosition(index).position;
                
                Camp.Spawn(adventurerIndex, profitPosition, venturerHierarchy, out var vb);
                VenturerList.Add(vb);
            });
        }

        private void SpawnVillain(VillainType villainIndex)
        {
            var spawnPosition = RaidDirector.StageDirector.VillainSpawnPosition.position;

            Den.Spawn(villainIndex, spawnPosition, villainHierarchy, out var vb);

            var reward = Den.GetReward(villainIndex);
            
            Villain                    =  vb;
            
            // TODO.TEMP TEST
            var x = Random.value * Random.Range(1.0f, 3.0f);
            var z = Random.value * Random.Range(1.0f, 3.0f);
            var offset = new Vector3(x, 0f, z);
            Villain.transform.position += offset;
            // 
            
            Villain.AddReward(() =>
            {
                Den.GetVillainData(villainIndex).KillCount++;
                Camp.AddGrowMaterials(reward);
                
                // TEMP
                reward.ForEach(ingredient => Debug.Log($"Get {ingredient.Type} : {ingredient.Count}"));
            });
        }
    }
}