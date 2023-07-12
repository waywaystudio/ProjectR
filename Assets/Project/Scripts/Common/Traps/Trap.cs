using System.Collections.Generic;
using Common.Effects.Impulses;
using Common.Effects.Particles;
using Common.Execution;
using UnityEngine;

namespace Common.Traps
{
    public abstract class Trap : MonoBehaviour, IActionSender, ICombatSequence, IHasTaker, IEditable
    {
        [SerializeField] protected DataIndex trapCode;
        [SerializeField] protected HitExecutor hitExecutor;
        [SerializeField] protected FireExecutor fireExecutor;
        [SerializeField] protected List<CombatParticle> combatParticles;
        [SerializeField] protected List<CombatImpulse> combatImpulses;
        [SerializeField] protected TrapProlongTimer prolongTimer;
        [SerializeField] protected OldTimeTrigger delayTimer;
        [SerializeField] protected Vector3 sizeVector;
        [SerializeField] protected LayerMask targetLayer;

        public DataIndex DataIndex => trapCode;
        public ICombatProvider Provider { get; protected set; }
        public ICombatTaker Taker { get; protected set; }
        public bool IsReady => delayTimer.IsPulled;
        public float Distance => sizeVector.x;
        public float Radius => sizeVector.y;
        public float Angle => sizeVector.z;
        public float ProlongTime => prolongTimer.CastingTime;
        public Vector3 SizeVector => sizeVector;
        public LayerMask TargetLayer => targetLayer;
        
        public CombatSequence Sequence { get; } = new();
        public CombatSequenceBuilder Builder { get; private set; }
        public CombatSequenceInvoker Invoker { get; private set; }


        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;

            Invoker = new CombatSequenceInvoker(Sequence);
            Builder = new CombatSequenceBuilder(Sequence);

            Builder
                .Add(Section.Active, "PullDelayTimer", delayTimer.Pull);
            
            hitExecutor.Initialize(Sequence, this);
            fireExecutor.Initialize(Sequence, this);
            prolongTimer.Initialize(this);
            combatParticles?.ForEach(cp => cp.Initialize(Sequence, this));
            combatImpulses?.ForEach(ci => ci.Initialize(Sequence));
        }

        protected virtual void Dispose()
        {
            /* Annotation
             * SKillComponent와 다르게 Pooling 기반 Object는 Dispose에서 Invoker.End실행 불가.
             * Pool을 가진 Object가 먼저 파괴되었을 수 있음
             * Invoker.End(); */
            Sequence.Clear();
            prolongTimer.Dispose();
            delayTimer.Dispose();
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
            
            GetComponentsInChildren(combatParticles);
            GetComponentsInChildren(combatImpulses);
        }
#endif
    }
}
