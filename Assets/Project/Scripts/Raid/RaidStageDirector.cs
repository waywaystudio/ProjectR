using System.Collections.Generic;
using Core;
using Raid.Stage;
using UnityEngine;

namespace Raid
{
    public class RaidStageDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private Transform bossSpawn;
        [SerializeField] private List<Transform> adventurerSpawnPositionList = new();
        
        public Transform BossSpawn => bossSpawn;

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
                        bossSpawn = position.transform;
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
