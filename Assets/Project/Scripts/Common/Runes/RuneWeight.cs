using System;
using UnityEngine;

namespace Common.Runes
{
    [Serializable]
    public class RewardRuneWeight
    {
        [SerializeField] private RewardRuneType rewardRuneType;
        [SerializeField] private int weight;
        
        public RewardRuneType RewardRuneType => rewardRuneType;
        public int Weight => weight;
    }

    [Serializable] 
    public class TaskRuneWeight
    {
        [SerializeField] private TaskRuneType taskRuneType;
        [SerializeField] private int weight;
        
        public TaskRuneType TaskRuneType => taskRuneType;
        public int Weight => weight;
    }
}
