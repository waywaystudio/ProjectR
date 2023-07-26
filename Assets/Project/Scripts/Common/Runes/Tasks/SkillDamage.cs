namespace Common.Runes.Tasks
{
    public class SkillDamage : TaskRune
    {
        public DataIndex TargetSkillIndex { get; private set; }

        public SkillDamage(DataIndex skill, float targetDamage)
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
