using System;
using Common.Effects;
using Common.Execution;
using UnityEngine;

namespace Common.Projectiles
{
    public class Projectile : MonoBehaviour, ICombatObject, IEditable
    {
        [SerializeField] protected DataIndex projectileCode;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected HitExecutor hitExecutor;
        [SerializeField] protected FireExecutor fireExecutor;
        [SerializeField] protected Effector effector;
        [SerializeField] protected Trajectory trajectory;
        
        public ICombatProvider Provider { get; protected set; }
        public ICombatTaker Taker { get; protected set; }
        public DataIndex DataIndex => projectileCode;
        
        public Func<float> Haste => () => Provider is not null ? Provider.StatTable.Haste : 0f;
        public HitExecutor HitExecutor => hitExecutor;
        public CombatSequence Sequence { get; } = new();
        public CombatSequenceBuilder Builder { get; private set; }
        public CombatSequenceInvoker Invoker { get; private set; }
        

        /// <summary>
        /// Create Pooling에서 호출
        /// 보통 SkillSequence에 Execution으로 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;
            Invoker  = new CombatSequenceInvoker(Sequence);
            Builder  = new CombatSequenceBuilder(Sequence);

            trajectory.Initialize(this);
            hitExecutor.Initialize(this);
            fireExecutor.Initialize(this);
            effector.Initialize(this);
        }
        

        protected virtual void Dispose()
        {
            trajectory.Dispose();
            Sequence.Clear();
        }

        private void OnDestroy()
        {
            Dispose();
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            hitExecutor.GetExecutionInEditor(transform);
            fireExecutor.GetExecutionInEditor(transform);
            effector.GetEffectsInEditor(transform);
        }
#endif
    }
}
