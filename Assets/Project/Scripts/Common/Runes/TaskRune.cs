using System;
using Common.Characters;

namespace Common.Runes
{
    public abstract class TaskRune
    {
        public FloatEvent Progress { get; } = new();
        public float Max { get; protected set; } = 1f;
        public bool IsSuccess { get; protected set; }
        
        protected CharacterBehaviour Tasker { get; set; }

        public void Assign(CharacterBehaviour tasker)
        {
            Tasker = tasker;
        }

        public void Dismissal()
        {
            Tasker = null;
        }

        public abstract void ActiveTask();
        public abstract void Accomplish();
        public abstract void Defeat();
    }
}
