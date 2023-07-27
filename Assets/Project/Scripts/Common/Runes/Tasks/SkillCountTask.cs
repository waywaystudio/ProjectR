using System;

namespace Common.Runes.Tasks
{
    [Serializable]
    public class SkillCountTask : TaskRune
    {
        public DataIndex TargetSkillIndex; 
        
        public override TaskRuneType RuneType => TaskRuneType.SkillCount;
        public override string Description => $"Use {TargetSkillIndex} {Max} times in a single battle.";
        

        public SkillCountTask(DataIndex skill, int count)
        {
            TargetSkillIndex = skill;
            Max              = count;
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
            IsSuccess = Progress.Value >= Max;
        }

        public override void Defeat()
        {
            
        }
        
        
        private void AddHitCount()
        {
            Progress.Value = Progress.Value < Max
                ? Progress.Value + 1f
                : Progress.Value;
        }
    }
}
