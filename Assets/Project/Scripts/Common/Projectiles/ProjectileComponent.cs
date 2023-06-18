using Common.Execution;
using Common.Systems;
using UnityEngine;

namespace Common.Projectiles
{
    public class ProjectileComponent : MonoBehaviour, IActionSender, IEditable
    {
        [SerializeField] protected Executor executor;
        [SerializeField] protected Sequencer sequencer;
        [SerializeField] protected CollidingSystem collidingSystem;
        [SerializeField] protected DataIndex projectileCode;
        [SerializeField] protected LayerMask targetLayer;
        
        public ICombatProvider Provider { get; protected set; }
        public DataIndex DataIndex => projectileCode;
        public LayerMask TargetLayer => targetLayer;
        public Sequencer Sequencer => sequencer;
        
        // TODO. SequenceBuilder, Invoker가 Initialize 되기전에 호출되는 경우가 있음.
        // 구조 전환과정에서 생기는 오류고, 모두 수정되면 간단하게 get, new(); 로 처리 
        private readonly SequenceBuilder sequenceBuilder = new();
        private readonly SequenceInvoker sequenceInvoker = new();
        public SequenceBuilder SequenceBuilder
        {
            get
            {
                if (!sequenceBuilder.IsInitialized) sequenceBuilder.Initialize(sequencer);
                return sequenceBuilder;
            }
        }
        public SequenceInvoker SequenceInvoker
        {
            get
            {
                if (!sequenceInvoker.IsInitialized) sequenceInvoker.Initialize(sequencer);
                return sequenceInvoker;
            }
        }

        /// <summary>
        /// Create Pooling에서 호출
        /// 보통 SkillSequence에 Execution으로 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;

            SequenceInvoker.Initialize(sequencer);
            SequenceBuilder.Initialize(sequencer)
                           .Add(SectionType.End, "ProjectileObjectActiveFalse", () => gameObject.SetActive(false));
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
        public void Dispose()
        {
            sequencer.Clear();
            
            Destroy(gameObject);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            executor.EditorGetExecutions(gameObject);
        }
#endif
    }
}
