namespace Common.Runes.Tasks
{
    public class SkillCount : TaskRune
    {
        public DataIndex TargetSkillIndex { get; private set; }

        public SkillCount(DataIndex skill, int count)
        {
            TargetSkillIndex = skill;
            Max.Value        = count;
        }
        
        public override void ActiveTask()
        {
            var targetSkill = Tasker.SkillTable[TargetSkillIndex];
            if (targetSkill is null) return;

            var builder = new CombatSequenceBuilder(targetSkill.Sequence);

            builder
                .AddHit("UpdateTask", _ => AddHitCount())
                ;
        }

        public override void Accomplish()
        {
            IsSuccess = Progress.Value >= Max.Value;
        }

        public override void Defeat()
        {
            
        }
        
        
        private void AddHitCount()
        {
            Progress.Value = Progress.Value < Max.Value
                ? Progress.Value++
                : Progress.Value;
        }
    }
}
