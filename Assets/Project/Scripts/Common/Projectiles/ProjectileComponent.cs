using System.Collections.Generic;
using Common.Execution;
using Common.Particles;
using UnityEngine;

namespace Common.Projectiles
{
    public class ProjectileComponent : MonoBehaviour, IActionSender, ICombatSequence, IHasTaker, IEditable
    {
        [SerializeField] protected DataIndex projectileCode;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected Trajectory trajectory;
        [SerializeField] protected HitExecutor hitExecutor;
        [SerializeField] protected FireExecutor fireExecutor;
        [SerializeField] protected List<CombatParticle> combatParticleList;
        
        public ICombatProvider Provider { get; protected set; }
        public ICombatTaker Taker { get; protected set; }
        public DataIndex DataIndex => projectileCode;
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

            Invoker = new CombatSequenceInvoker(Sequence);
            Builder = new CombatSequenceBuilder(Sequence);
            Builder
                .Add(Section.End, "ProjectileObjectActiveFalse", () => gameObject.SetActive(false));
            
            trajectory.Initialize(this);
            hitExecutor.Initialize(Sequence, this);
            fireExecutor.Initialize(Sequence, this);
            combatParticleList.ForEach(particle => particle.Initialize(Sequence, this));
        }


        /// <summary>
        /// Scene이 종료되거나, 설정된 Pool 개수를 넘어서 생성된 상태이상효과가 만료될 때 호출
        /// </summary>
        protected virtual void Dispose()
        {
            Sequence.Clear();
            combatParticleList?.ForEach(cp => cp.Dispose());
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
            
            GetComponentsInChildren(combatParticleList);
        }
#endif
    }
}
