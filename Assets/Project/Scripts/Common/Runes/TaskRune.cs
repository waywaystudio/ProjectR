using System;
using Common.Characters;

namespace Common.Runes
{
    [Serializable]
    public abstract class TaskRune
    {
        public float Max = 1f;
        public FloatEvent Progress = new ();
        
        public abstract TaskRuneType RuneType { get; }
        public abstract string Description { get; }
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
