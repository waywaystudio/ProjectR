using Common.Execution;
using Common.Systems;
using UnityEngine;

namespace Common.Projectiles
{
    public class ProjectileComponent : MonoBehaviour, ISections, IActionSender, IEditable
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
        public ConditionTable Condition => sequencer.Condition;
        public ActionTable ActiveAction => sequencer.ActiveAction;
        public ActionTable CancelAction => sequencer.CancelAction;
        public ActionTable CompleteAction => sequencer.CompleteAction;
        public ActionTable EndAction => sequencer.EndAction;

        /// <summary>
        /// Create Pooling에서 호출
        /// 보통 SkillSequence에 Execution으로 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;
            
            sequencer.EndAction.Add("ProjectileObjectActiveFalse", () => gameObject.SetActive(false));
        }
        
        /// <summary>
        /// 성공적으로 스킬 사용시 호출.
        /// </summary>
        public void Activate()
        {
            gameObject.SetActive(true);
            sequencer.Active();
        }

        /// <summary>
        /// 스킬의 가동범위로 부터 대상을 받아서
        /// 데미지, 상태이상 부여 등을 실제 수행하는 함수
        /// </summary>
        public virtual void Execution() { }


        /// <summary>
        /// 해제 시 호출. (만료 아님)
        /// </summary>
        public void Cancel() => sequencer.Cancel();

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
            executor = GetComponentInChildren<Executor>();
        }
#endif
    }
}
