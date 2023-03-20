using System.Collections.Generic;
using Common.Execution;
using Common.Systems;
using DG.Tweening;
using UnityEngine;

namespace Common.Traps
{
    /// 반대 개념 항상 존재. (딜 <-> 힐)
    // 밟는 동안 효과
    /// 효과 종류
    // 데미지, 힐, 디버프, 버프
    public abstract class TrapComponent : MonoBehaviour, ISequence, IExecutable, IEditable
    {
        [SerializeField] private CollidingSystem collidingSystem;
        [SerializeField] protected DataIndex trapCode;
        [SerializeField] protected float delayTime;
        [SerializeField] protected float radius;
        [SerializeField] protected float prolongTime;
        [SerializeField] protected LayerMask targetLayer;

        public ICombatProvider Provider { get; protected set; }
        public DataIndex ActionCode => trapCode;
        public float ProlongTime => prolongTime;
        public float Radius => radius;
        public LayerMask TargetLayer => targetLayer;
        public FloatEvent ProgressTime { get; } = new(0f, float.PositiveInfinity);
        
        public ActionTable OnActivated { get; } = new();
        public ActionTable OnCanceled { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnEnded { get; } = new();
        public ExecutionTable ExecutionTable { get; } = new();
        
        /// <summary>
        /// Scene 시작과 함께 한 번 호출.
        /// 보통 SkillSequence에 StatusCompletion으로 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider, Vector3 position)
        {
            Provider           = provider;
            transform.position = position;
        }
        
        /// <summary>
        /// 성공적으로 스킬 사용시 호출.
        /// </summary>
        public void Activate()
        {
            gameObject.SetActive(true);
            ProgressTime.Value = 0f;

            if (delayTime != 0f)
            {
                DOVirtual.DelayedCall(delayTime, OnActivated.Invoke);
            }
            else
            {
                OnActivated.Invoke();
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
        public void Cancel()
        {
            OnCanceled.Invoke();
            
            End();
        }
        
        /// <summary>
        /// 성공적으로 만료시 호출
        /// </summary>
        public void Complete()
        {
            OnCompleted.Invoke();

            End();
        }
        
        /// <summary>
        /// 만료시 호출 (성공 실패와 무관)
        /// </summary>
        protected void End()
        {
            gameObject.SetActive(false);
            OnEnded.Invoke();
        }
        
        /// <summary>
        /// Scene이 종료되거나, 설정된 Pool 개수를 넘어서 생성된 상태이상효과가 만료될 때 호출
        /// </summary>
        public void Dispose()
        {
            this.Clear();
            
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
