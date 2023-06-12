using Common;
using UnityEngine;
using UnityEngine.Events;

namespace Character.Villains
{
    public abstract class VillainPhase : MonoBehaviour
    {
        [SerializeField] protected bool isLast;
        [SerializeField] protected int index;
        [SerializeField] protected VillainPhase nextPhase;
        [SerializeField] protected UnityEvent onActiveEvent;
        [SerializeField] protected UnityEvent onCompleteEvent;
        [SerializeField] protected UnityEvent onEndEvent;

        private VillainBehaviour villain;

        public ConditionTable Conditions { get; } = new();
        public ActionTable OnActivated { get; } = new();
        public ActionTable OnCanceled { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnEnded { get; } = new();

        public VillainPhaseMask PhaseFlag => (VillainPhaseMask)(1 << index);
        public bool IsProgress { get; private set; }
        public bool IsEnd { get; private set; } = true;
        public bool IsLastPhase => isLast;
        public int Index => index;
        public VillainPhase NextPhase => nextPhase;
        protected VillainBehaviour Villain => villain ??= GetComponentInParent<VillainBehaviour>();

        public void TryToNextPhase()
        {
            if (!IsLastPhase && IsProgress && !IsEnd && IsAbleToNextPhase())
            {
                Complete();
            }
        }

        public bool IsAbleToNextPhase()
        {
            return Conditions.IsAllTrue;
        }

        public void Activate()
        {
            IsProgress = true;
            IsEnd      = false;
            
            onActiveEvent?.Invoke();
            OnActivated.Invoke();
        }

        public void Cancel()
        {
            IsProgress = false;
            
            OnCanceled.Invoke();
        }

        public void Complete()
        {
            IsProgress = false;
            
            onCompleteEvent?.Invoke();
            OnCompleted.Invoke();
        }

        public void End()
        {
            onEndEvent?.Invoke();
            OnEnded.Invoke();
            
            IsEnd = true;
            
            nextPhase.Activate();
            villain.CurrentPhase = nextPhase;
        }
    }
}
