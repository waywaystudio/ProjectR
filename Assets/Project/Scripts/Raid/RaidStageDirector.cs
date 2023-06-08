using System.Collections.Generic;
using Raid.Stage;
using UnityEngine;
using UnityEngine.Serialization;

namespace Raid
{
    public class RaidStageDirector : MonoBehaviour, IEditable
    {
        [FormerlySerializedAs("bossSpawn")] [SerializeField] private Transform villainSpawnPosition;
        [SerializeField] private List<Transform> adventurerSpawnPositionList = new();
        
        public Transform VillainSpawnPosition => villainSpawnPosition;

        public Transform GetAdventurerPosition(int index)
        {
            if (adventurerSpawnPositionList.Count <= index)
            {
                Debug.LogWarning($"Require More Adventurer Spawn Position. Limit:{adventurerSpawnPositionList.Count}, Input:{index}");
                return null;
            }

            return adventurerSpawnPositionList[index];
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            var spawnPositionList = GetComponentsInChildren<SpawnTransform>();
            
            spawnPositionList.ForEach(position =>
            {
                switch (position.IconName)
                {
                    case "Boss":
                        villainSpawnPosition = position.transform;
                        break;
                    case "Adventurer":
                        adventurerSpawnPositionList.Add(position.transform);
                        break;
                }
            });
        }
#endif
    }
}
