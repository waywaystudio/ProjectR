using System.Collections.Generic;
using Common.Effects.Particles;
using Common.Effects.Sounds;
using Common.Execution;
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
        [SerializeField] protected List<CombatSound> combatSounds;
        
        public ICombatProvider Provider { get; protected set; }
        public ICombatTaker Taker { get; protected set; }
        public DataIndex DataIndex => projectileCode;
        
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

            Invoker = new CombatSequenceInvoker(Sequence);
            Builder = new CombatSequenceBuilder(Sequence);
            Builder
                .Add(Section.End, "ProjectileObjectActiveFalse", () => gameObject.SetActive(false));
            
            trajectory.Initialize(this);
            hitExecutor.Initialize(Sequence, this);
            fireExecutor.Initialize(Sequence, this);
            
            combatParticleList.ForEach(particle => particle.Initialize(Sequence, this));
            combatSounds?.ForEach(cs => cs.Initialize(Sequence));
        }
        

        protected virtual void Dispose()
        {
            Sequence.Clear();
            trajectory.Dispose();
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
            GetComponentsInChildren(combatSounds);
        }
#endif
    }
}
