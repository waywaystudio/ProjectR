using UnityEngine;

namespace Common.Runes.Tasks
{
    public class SkillDamage : TaskRune
    {
        public DataIndex TargetSkillIndex { get; private set; }

        public SkillDamage(DataIndex skill, float targetDamage)
        {
            TargetSkillIndex = skill;
            Max.Value        = targetDamage;
        }
        
        public override void ActiveTask()
        {
            var targetSkill = Tasker.SkillTable[TargetSkillIndex];
            if (targetSkill is null) return;

            Tasker.OnCombatProvided.Add("DamageAccumulation", AccumulateSkillDamage);
            // var builder = new CombatSequenceBuilder(targetSkill.Sequence);
            //
            // builder
            //     .AddHit("UpdateTask", _ => AddHitCount())
            //     ;
        }

        public override void Accomplish()
        {
            IsSuccess = Progress.Value >= Max.Value;
        }

        public override void Defeat()
        {
            
        }
        
        
        private void AccumulateSkillDamage(CombatLog log)
        {
            if (log.CombatIndex != TargetSkillIndex) return;

            Progress.Value = Progress.Value + log.Value < Max.Value 
                ? Progress.Value + log.Value 
                : Max.Value;
        }
    }
}
