using System;

namespace Common.Runes.Tasks
{
    [Serializable]
    public class SkillDamageTask : TaskRune
    {
        public DataIndex TargetSkillIndex;
        
        public override TaskRuneType RuneType => TaskRuneType.SkillDamage;
        public override string Description => $"Take {Max} damage by {TargetSkillIndex} in a single battle.";
        

        public SkillDamageTask(DataIndex skill, float targetDamage)
        {
            TargetSkillIndex = skill;
            Max              = targetDamage;
        }
        
        public override void ActiveTask()
        {
            var targetSkill = Tasker.SkillTable[TargetSkillIndex];
            if (targetSkill is null) return;

            Tasker.OnCombatProvided.Add("DamageAccumulation", AccumulateSkillDamage);
        }

        public override void Accomplish()
        {
            IsSuccess = Progress.Value >= Max;
        }

        public override void Defeat()
        {
            
        }
        
        
        private void AccumulateSkillDamage(CombatLog log)
        {
            if (log.CombatIndex != TargetSkillIndex) return;

            Progress.Value = Progress.Value + log.Value < Max 
                ? Progress.Value + log.Value 
                : Max;
        }
    }
}
