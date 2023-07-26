using System;

namespace Common.Runes.Tasks
{
    [Serializable]
    public class Victory : TaskRune
    {
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
