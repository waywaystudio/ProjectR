using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class SkillSequencer : Sequencer<Vector3>
    {
        public ActionTable ExecuteAction { get; } = new();

        public void Execution()
        {
            ExecuteAction.Invoke();
        }
    }
    
    [Serializable]
    public class StatusEffectSequencer : Sequencer<ICombatTaker>
    {
        public ActionTable ExecuteAction { get; } = new();

        public void Execution()
        {
            ExecuteAction.Invoke();
        }
    }
    
    [Serializable]
    public class TrapSequencer : Sequencer<Vector3>
    {
        public ActionTable ExecuteAction { get; } = new();

        public void Execution()
        {
            ExecuteAction.Invoke();
        }
    }
}
