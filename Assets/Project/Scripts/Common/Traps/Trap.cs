using System;
using Common.Effects;
using Common.Execution;
using UnityEngine;

namespace Common.Traps
{
    public abstract class Trap : MonoBehaviour, ICombatObject, IEditable
    {
        [SerializeField] protected DataIndex trapCode;
        [SerializeField] protected HitExecutor hitExecutor;
        [SerializeField] protected FireExecutor fireExecutor;
        [SerializeField] protected Effector effector;
        [SerializeField] protected TrapProlongTimer prolongTimer;
        [SerializeField] protected SizeEntity sizeEntity;
        [SerializeField] protected LayerMask targetLayer;

        public DataIndex DataIndex => trapCode;
        public ICombatProvider Provider { get; protected set; }
        public ICombatTaker Taker { get; protected set; }
        public float Distance => sizeEntity.PivotRange;
        public float Radius => sizeEntity.AreaRange;
        public float Angle => sizeEntity.Angle;
        public float ProlongTime => prolongTimer.Duration;
        public SizeEntity SizeEntity => sizeEntity;
        public LayerMask TargetLayer => targetLayer;
        public Func<float> Haste => () => Provider is not null ? Provider.StatTable.Haste : 0f;
        
        public CombatSequence Sequence { get; } = new();
        public CombatSequenceBuilder Builder { get; private set; }
        public CombatSequenceInvoker Invoker { get; private set; }


        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;

            Invoker = new CombatSequenceInvoker(Sequence);
            Builder = new CombatSequenceBuilder(Sequence);
            
            hitExecutor.Initialize(this);
            fireExecutor.Initialize(this);
            prolongTimer.Initialize(this);
            effector.Initialize(this);
        }

        protected virtual void Dispose()
        {
            Sequence.Clear();
            prolongTimer.Dispose();
        }


        private void OnDestroy()
        {
            Dispose();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            hitExecutor.GetExecutionInEditor(transform);
            fireExecutor.GetExecutionInEditor(transform);
            effector.GetEffectsInEditor(transform);
        }
#endif
    }
}
