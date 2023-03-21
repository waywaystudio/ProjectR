using System.Collections.Generic;
using Common.Execution;
using Common.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Projectiles
{
    
    /// Trajectory Focus
    // Instant
    // Parabola
    
    /// Destination Focus
    // Direction
    // Position
    
    /// Execution Focus
    // Damage, Heal, Status, Trap
    
    /// Completion Focus
    // Bounce, Yoyo, 
    public class ProjectileComponent : MonoBehaviour, ISequence, IExecutable
    {
        [SerializeField] protected CollidingSystem collidingSystem;
        [SerializeField] protected DataIndex projectileCode;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected float radius;
        
        protected DataIndex ActionCode => projectileCode;
        
        public ICombatProvider Provider { get; protected set; }
        public LayerMask TargetLayer => targetLayer;

        public ActionTable OnActivated { get; } = new();
        public ActionTable OnCanceled { get; } = new();
        [ShowInInspector]
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnEnded { get; } = new();
        public ExecutionTable ExecutionTable { get; } = new();

        /// <summary>
        /// Create Pooling에서 호출
        /// 보통 SkillSequence에 Execution으로 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;
        }
        
        /// <summary>
        /// 성공적으로 스킬 사용시 호출.
        /// </summary>
        public void Activate()
        {
            gameObject.SetActive(true);
            OnActivated.Invoke();
        }

        /// <summary>
        /// 스킬의 가동범위로 부터 대상을 받아서
        /// 데미지, 상태이상 부여 등을 실제 수행하는 함수
        /// </summary>
        public virtual void Execution() { }

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
        public virtual void Complete()
        {
            OnCompleted.Invoke();

            End();
        }

        /// <summary>
        /// 만료시 호출 (성공 실패와 무관)
        /// </summary>
        public void End()
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
    }
}
