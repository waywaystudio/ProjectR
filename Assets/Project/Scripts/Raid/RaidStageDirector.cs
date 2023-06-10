using System.Collections.Generic;
using Raid.Stage;
using UnityEngine;

namespace Raid
{
    public class RaidStageDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private Transform villainSpawnPosition;
        [SerializeField] private List<Transform> VenturerSpawnPositionList = new();
        
        public Transform VillainSpawnPosition => villainSpawnPosition;

        public Transform GetAdventurerPosition(int index)
        {
            if (VenturerSpawnPositionList.Count <= index)
            {
                Debug.LogWarning($"Require More Adventurer Spawn Position. Limit:{VenturerSpawnPositionList.Count}, Input:{index}");
                return null;
            }

            return VenturerSpawnPositionList[index];
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            VenturerSpawnPositionList.Clear();
            
            var spawnPositionList = GetComponentsInChildren<SpawnTransform>();
            
            spawnPositionList.ForEach(position =>
            {
                switch (position.IconName)
                {
                    case "Villain":
                        villainSpawnPosition = position.transform;
                        break;
                    case "Venturer":
                        VenturerSpawnPositionList.Add(position.transform);
                        break;
                }
            });
        }
#endif
    }
}
