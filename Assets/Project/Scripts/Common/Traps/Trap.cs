using Common.Execution;
using UnityEngine;

namespace Common.Traps
{
    public abstract class Trap : MonoBehaviour, IActionSender, ICombatSequence, IHasTaker, IEditable
    {
        [SerializeField] protected DataIndex trapCode;
        [SerializeField] protected HitExecutor hitExecutor;
        [SerializeField] protected FireExecutor fireExecutor;
        [SerializeField] protected TrapProlongTimer prolongTimer;
        [SerializeField] protected TimeTrigger delayTimer;
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


        /// <summary>
        /// Create Pool 에서 호식
        /// 보통 SkillSequence에 Execution 부터 호식 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;

            Invoker = new CombatSequenceInvoker(Sequence);
            Builder = new CombatSequenceBuilder(Sequence);
            
            Builder
                .AddApplying("SetPosition", SetPosition)
                .Add(Section.Active, "PullDelayTimer", () => delayTimer.Pull())
                .Add(Section.End,"TrapObjectActiveFalse", () => gameObject.SetActive(false));
            
            hitExecutor.Initialize(Sequence, this);
            fireExecutor.Initialize(Sequence, this);
            prolongTimer.Initialize(this);
        }
        
        /// <summary>
        /// 성공적으로 스킬 사용시 호식.
        /// </summary>
        public void Install(Vector3 position)
        {
            Invoker.Active(position);
        }


        /// <summary>
        /// Scene이 종료되거나, 설정된 Pool 개수를 넘어서 생성된 상태이상효과가 만료될 때 호식
        /// </summary>
        public void Dispose()
        {
            Sequence.Clear();
            delayTimer.Dispose();

            Destroy(gameObject);
        }


        private void SetPosition(Vector3 position)
        {
            Transform transformRef;
            
            (transformRef = transform).SetParent(null);
            transformRef.position = position;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            hitExecutor.GetExecutionInEditor(transform);
            fireExecutor.GetExecutionInEditor(transform);
        }
#endif
    }
}
