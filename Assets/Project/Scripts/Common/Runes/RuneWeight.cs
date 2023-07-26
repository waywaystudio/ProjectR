using System;
using UnityEngine;

namespace Common.Runes
{
    [Serializable]
    public class RuneWeight
    {
        [SerializeField] private RuneType runeType;
        [SerializeField] private int weight;
        
        public RuneType RuneType => runeType;
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
