using System;
using Common.Characters;
using UnityEngine;

namespace Common.Runes
{
    [Serializable]
    public class EthosRune
    {
        public int Grade;                          
        public TaskRune TaskRune;
        public RewardRune RewardRune;
        
        private Sprite icon;

        public bool IsCompleteTask => TaskRune.IsSuccess;
        public float TaskGoal => TaskRune.Max;
        public string TaskDescription => TaskRune.Description;
        public FloatEvent TaskProgress => TaskRune.Progress;
        public Sprite Icon => icon ??= Database.RuneSpriteData.Get(RewardRune.RuneType.ToDataIndex());

        public EthosRune(int grade, TaskRune taskRune, RewardRune rewardRune)
        {
            Grade      = grade;
            TaskRune   = taskRune;
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
    }
}
