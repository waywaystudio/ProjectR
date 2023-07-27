using System;

namespace Common.Runes.Tasks
{
    [Serializable]
    public class VictoryTask : TaskRune
    {
        public override TaskRuneType RuneType => TaskRuneType.Victory;
        public override string Description => $"Defeat a Villain.";
        
        public override void ActiveTask()
        {
            Progress.Value = 0f;
            Max            = 1f;
        }

        public override void Accomplish()
        {
            Progress.Value = 1f;
            IsSuccess      = true;
        }

        public override void Defeat()
        {
            
        }
    }
}
