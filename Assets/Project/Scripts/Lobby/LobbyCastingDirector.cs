using Character.Adventurers;
using UnityEngine;

namespace Lobby
{
    public class LobbyCastingDirector : MonoBehaviour
    {
        [SerializeField] private Transform knightSpawnTransform;
        [SerializeField] private Transform adventurerObjectHierarchy;
        
        public void Initialize()
        {
            var knightIndex = DataIndex.Knight;
            
            if (!Database.CombatClassPrefabData.Get<Adventurer>(knightIndex, out var adventurerPrefab)) return;
                
            var profitPosition = knightSpawnTransform.position;
            var knight = Instantiate(adventurerPrefab, profitPosition, Quaternion.identity,
                                     adventurerObjectHierarchy);
            
            knight.gameObject.SetActive(true);
            knight.ForceInitialize();

            LobbyDirector.Knight = knight;
        }
    }
}
