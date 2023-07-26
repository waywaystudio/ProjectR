using System;
using Common.Characters;
using UnityEngine;

namespace Common.Runes
{
    [Serializable]
    public class EthosRune
    {
        public int Grade { get; set; }
        public Sprite Icon { get; set; }
        public TaskRune TaskRune { get; set; }
        public RewardRune RewardRune { get; set; }

        public RuneType RuneType => RewardRune.RuneType;
        public bool IsComplete => TaskRune.IsSuccess;
        public FloatEvent Progress => TaskRune.Progress;
        public float Max => TaskRune.Max;
        public string TaskDescription => "Currently Not Define";

        public EthosRune(int grade, TaskRune taskRune, RewardRune rewardRune)
        {
            Grade = grade;
            // Icon = Database.RuneSprite.Get(RewardRuneType)
            TaskRune = taskRune;
            RewardRune = rewardRune;
        }


        public void Assign(CharacterBehaviour cb) => TaskRune.Assign(cb);
        public void Dismissal() => TaskRune.Dismissal();
        public void ActiveTask() => TaskRune.ActiveTask();
        public void Accomplish() => TaskRune.Accomplish();
        public void Defeat() => TaskRune.Defeat();

        public void Disassemble()
        {
            // by grade
        }

        public void Combine()
        {
            // by grade
        }

        public void GetReward()
        {
            if (!IsComplete) return;
            
            // RewardRune.GetReward();
        }
    }
}
