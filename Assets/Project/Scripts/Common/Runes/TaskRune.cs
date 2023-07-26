using System;
using Common.Characters;

namespace Common.Runes
{
    public abstract class TaskRune
    {
        public Observable<float> Progress { get; } = new();
        public Observable<float> Max { get; } = new();
        public bool IsSuccess { get; protected set; }
        // => Math.Abs(Progress.Value / Max.Value - 1f) < 0.0001f;
        
        protected CharacterBehaviour Tasker { get; set; }

        public virtual void Assign(CharacterBehaviour tasker)
        {
            Tasker = tasker;
        }

        public virtual void Dismissal()
        {
            Tasker = null;
        }

        public abstract void ActiveTask();
        public abstract void Accomplish();
        public abstract void Defeat();
    }
}
