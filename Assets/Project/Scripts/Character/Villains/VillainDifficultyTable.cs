using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Character.Villains
{
    public class VillainDifficultyTable : MonoBehaviour
    {
        [SerializeField] private List<StatEntity> hardStatEntityList;
        [SerializeField] private List<StatEntity> mythicStatEntityList;

        public StatSpec GetDifficultySpec(DifficultyType difficulty, int level)
        {
            var result = new StatSpec();

            switch (difficulty)
            {
                case DifficultyType.Normal: break;
                case DifficultyType.Hard:
                {
                    hardStatEntityList.ForEach(stat => result.Add(stat.StatType, "Difficulty", stat.Value));
                    break;
                }
                case DifficultyType.Mythic:
                {
                    mythicStatEntityList.ForEach(stat => result.Add(stat.StatType, "Difficulty", stat.Value));
                    break;
                }
                default: return null;
            }
            
            result.IterateOverStats(stat => stat.Value *= 1 + level * 0.1f);

            return result;
        }
    }
}
