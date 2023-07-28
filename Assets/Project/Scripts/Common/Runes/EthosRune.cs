using System;
using Common.Characters;
using UnityEngine;

namespace Common.Runes
{
    [Serializable]
    public class EthosRune : IReward
    {
        [SerializeField] private int grade;
        [SerializeField] private string title;
        
        public TaskRune TaskRune;
        public RewardRune RewardRune;
        
        private Sprite icon;

        public bool IsCompleteTask => TaskRune.IsSuccess;
        public float TaskGoal => TaskRune.Max;
        public int Grade => grade;
        public string Title => title ??= $"{RewardRune.RuneType.ToString()}Rune";
        public string Description => TaskRune.Description;
        public Sprite Icon => icon ??= Database.RuneSpriteData.Get(RewardRune.RuneType.ToDataIndex());
        public FloatEvent TaskProgress => TaskRune.Progress;

        public EthosRune(int grade, TaskRune taskRune, RewardRune rewardRune)
        {
            this.grade = grade;
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
