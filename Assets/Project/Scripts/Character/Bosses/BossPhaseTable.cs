using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.Events;

namespace Character.Bosses
{
    public class BossPhaseTable : MonoBehaviour
    {
        private int currentPhase;
        
        public int CurrentPhase => currentPhase;

        private Dictionary<int, BossPhase> phaseTable = new();


        public bool IsAbleToNextPhase()
        {
            return phaseTable[currentPhase].IsAbleToNextPhase;
        }

        public void ActiveNextPhase()
        {
            var nextPhase = phaseTable[CurrentPhase].NextPhaseIndex;
            
            phaseTable[nextPhase].Activate();
            currentPhase = nextPhase;
        }
    }

    public class BossPhase : MonoBehaviour, IConditionalSequence
    {
        [SerializeField] private int index;
        [SerializeField] private UnityEvent onActiveEvent;
        [SerializeField] private UnityEvent onCompleteEvent;
        [SerializeField] private UnityEvent onEndEvent;

        public ConditionTable Conditions { get; } = new();
        public ActionTable OnActivated { get; } = new();
        public ActionTable OnCanceled { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnEnded { get; } = new();

        public int NextPhaseIndex => index + 1;
        public bool IsAbleToNextPhase => Conditions.IsAllTrue;

        public virtual void Activate() { }
        public virtual void Cancel() { }
        public virtual void Complete() { }
        public virtual void End() { }
    }
}
