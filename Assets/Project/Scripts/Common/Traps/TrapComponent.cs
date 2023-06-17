using System.Collections.Generic;
using Common.Execution;
using Common.Systems;
using DG.Tweening;
using UnityEngine;

namespace Common.Traps
{
    public abstract class TrapComponent : MonoBehaviour, IActionSender, ISections, IEditable
    {
        [SerializeField] protected Executor executor;
        [SerializeField] private TrapSequencer sequencer;
        [SerializeField] private CollidingSystem collidingSystem;
        [SerializeField] protected DataIndex trapCode;
        [SerializeField] protected float delayTime;
        [SerializeField] protected float radius;
        [SerializeField] protected LayerMask targetLayer;

        private readonly TrapSequenceBuilder sequenceBuilder = new();
        private readonly TrapSequenceInvoker sequenceInvoker = new();

        public ICombatProvider Provider { get; protected set; }
        public DataIndex DataIndex => trapCode;
        public float Radius => radius;
        public LayerMask TargetLayer => targetLayer;
        public float ProlongTime { get; set; }
        
        // public SkillExecutor Executor => executor;
        // public TrapSequencer TrapSequencer => sequencer;
        public TrapSequenceBuilder SequenceBuilder
        {
            get
            {
                if (!sequenceBuilder.IsInitialized) 
                    sequenceBuilder.Initialize(sequencer);
                
                return sequenceBuilder;
            }
        }

        public TrapSequenceInvoker SequenceInvoker
        {
            get
            {
                if (!sequenceInvoker.IsInitialized) 
                    sequenceInvoker.Initialize(sequencer);
                
                return sequenceInvoker;
            }
        }

        public ActionTable<Vector3> ActiveParamAction => sequencer.ActiveParamAction;
        public ConditionTable Condition => sequencer.Condition;
        public ActionTable ActiveAction => sequencer.ActiveAction;
        public ActionTable CancelAction => sequencer.CancelAction;
        public ActionTable CompleteAction => sequencer.CompleteAction;
        public ActionTable EndAction => sequencer.EndAction;
        public ActionTable ExecuteAction => sequencer.ExecuteAction;
        
        
        /// <summary>
        /// Create Pool 에서 호출
        /// 보통 SkillSequence에 Execution 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;
            
            SequenceInvoker.Initialize(sequencer);
            SequenceBuilder.Initialize(sequencer)
                           .AddEnd("TrapObjectActiveFalse", () => gameObject.SetActive(false));
        }
        
        /// <summary>
        /// 성공적으로 스킬 사용시 호출.
        /// </summary>
        public void Activate(Vector3 position)
        {
            Transform transform1;
            
            (transform1 = transform).SetParent(null);
            transform1.position = position;
            
            gameObject.SetActive(true);

            if (delayTime != 0f)
            {
                DOVirtual.DelayedCall(delayTime, () => SequenceInvoker.Active(position));
            }
            else
            {
                SequenceInvoker.Active(position);
            }
        }

        /// <summary>
        /// 스킬의 가동범위로 부터 대상을 받아서
        /// 데미지, 상태이상 부여 등을 실제 수행하는 함수
        /// </summary>
        public abstract void Execution();

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


        protected bool TryGetTakerInSphere(out List<ICombatTaker> takerList)
            => collidingSystem.TryGetTakersInSphere(transform.position, 
                radius, 
                360f, 
                targetLayer, 
                out takerList);


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            executor = GetComponentInChildren<Executor>();
            
            if (collidingSystem.IsNullOrDestroyed())
            {
                GameObject o;
                
                collidingSystem = (o = gameObject).AddComponent<CollidingSystem>();
                Debug.Log($"Add CollidingSystem Component In {o.name}");
            }
        }
#endif
    }
}
