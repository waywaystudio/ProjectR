using Common.Execution;
using DG.Tweening;
using UnityEngine;

namespace Common.Traps
{
    public abstract class TrapComponent : MonoBehaviour, IActionSender, ICombatSequence, IEditable
    {
        [SerializeField] protected DataIndex trapCode;
        [SerializeField] protected HitExecutor hitExecutor;
        [SerializeField] protected FireExecutor fireExecutor;
        [SerializeField] protected float delayTime;
        [SerializeField] protected float prolongTime;
        [SerializeField] protected Vector3 sizeVector;
        [SerializeField] protected LayerMask targetLayer;

        public ICombatProvider Provider { get; protected set; }
        public DataIndex DataIndex => trapCode;
        public float Distance => sizeVector.x;
        public float Radius => sizeVector.y;
        public float Angle => sizeVector.z;
        public float ProlongTime { get; set; }
        public Vector3 SizeVector => sizeVector;
        public LayerMask TargetLayer => targetLayer;
        
        public CombatSequence Sequence { get; } = new();
        public CombatSequenceBuilder Builder { get; private set; }
        public CombatSequenceInvoker Invoker { get; private set; }


        /// <summary>
        /// Create Pool 에서 호출
        /// 보통 SkillSequence에 Execution 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;

            Invoker = new CombatSequenceInvoker(Sequence);
            Builder = new CombatSequenceBuilder(Sequence);
            
            Builder
                .Add(Section.End,"TrapObjectActiveFalse", () => gameObject.SetActive(false));
            
            hitExecutor.Initialize(Sequence, this);
            fireExecutor.Initialize(Sequence, this);
        }
        
        /// <summary>
        /// 성공적으로 스킬 사용시 호출.
        /// </summary>
        public void Activate(Vector3 position)
        {
            transform.SetParent(null);
            transform.position = position;
            
            if (delayTime != 0f)
            {
                DOVirtual.DelayedCall(delayTime, () => Invoker.Active(position));
            }
            else
            {
                Invoker.Active(position);
            }
        }


        /// <summary>
        /// Scene이 종료되거나, 설정된 Pool 개수를 넘어서 생성된 상태이상효과가 만료될 때 호출
        /// </summary>
        public void Dispose()
        {
            Sequence.Clear();
            
            Destroy(gameObject);
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
