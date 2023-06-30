using Common.Execution;
using Common.Systems;
using UnityEngine;

namespace Common.Projectiles
{
    public class ProjectileComponent : MonoBehaviour, IActionSender, IEditable
    {
        [SerializeField] protected DataIndex projectileCode;
        [SerializeField] protected Executor executor;
        [SerializeField] protected Sequencer sequencer;
        [SerializeField] protected Trajectory trajectory;
        [SerializeField] protected CollidingSystem collidingSystem;
        [SerializeField] protected LayerMask targetLayer;
        
        public ICombatProvider Provider { get; protected set; }
        public DataIndex DataIndex => projectileCode;
        public LayerMask TargetLayer => targetLayer;

        public SequenceBuilder SequenceBuilder { get; private set; }
        public SequenceInvoker SequenceInvoker { get; private set; }

        /// <summary>
        /// Create Pooling에서 호출
        /// 보통 SkillSequence에 Execution으로 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;

            SequenceInvoker = new SequenceInvoker(sequencer);
            SequenceBuilder = new SequenceBuilder(sequencer);
            SequenceBuilder.Add(SectionType.End, "ProjectileObjectActiveFalse", () => gameObject.SetActive(false));
            
            trajectory.Initialize(this);
        }
        
        /// <summary>
        /// 성공적으로 스킬 사용시 호출.
        /// </summary>
        public void Activate()
        {
            gameObject.SetActive(true);
            SequenceInvoker.Active();
        }

        /// <summary>
        /// 스킬의 가동범위로 부터 대상을 받아서
        /// 데미지, 상태이상 부여 등을 실제 수행하는 함수
        /// </summary>
        public virtual void Execution() { }


        /// <summary>
        /// 해제 시 호출. (만료 아님)
        /// </summary>
        public void Cancel() => SequenceInvoker.Cancel();

        /// <summary>
        /// Scene이 종료되거나, 설정된 Pool 개수를 넘어서 생성된 상태이상효과가 만료될 때 호출
        /// </summary>
        protected virtual void Dispose()
        {
            sequencer.Clear();
        }
        

        private void OnDestroy()
        {
            Dispose();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            executor.EditorGetExecutions(gameObject);
        }
#endif
    }
}
