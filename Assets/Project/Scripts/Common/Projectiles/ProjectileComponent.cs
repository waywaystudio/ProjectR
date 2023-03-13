using DG.Tweening;
using UnityEngine;

namespace Common.Projectiles
{
    public class ProjectileComponent : MonoBehaviour, ISequence, IPoolable<ProjectileComponent>
    {
        /// OnActivated Focus
        // Trajectory Type.
        // Instantiate Type. (On Spot) // On Provider Position
        // Raycast pierce Type.
        // Chasing Type?

        /// OnCompleted Focus
        // Bounce Type.
        // Time Trigger
        // Trap Type. (Installed, if inCollider -> SingCompletion and End)
        // Area Type. (Installed, if inCollider -> tickCompletion and Prolong)
        // Trap, Area 모두 별도의 Component 느낌 아닌가?
        // Projectile은 그래도, 어딘가에서 발사하고, 날아가서 충돌하면 없어지는 싸이클이 있는데
        // Trap, Area는 라이프 사이클 생겨먹은게 다르다.
        // TrapComponent - CompletionType 으로 보는게 좋을 것 같다.

        protected ICombatProvider Provider;
        protected DataIndex ActionCode; 
        protected Tweener TrajectoryTweener;
        
        public Pool<ProjectileComponent> Pool { get; set; }
        
        protected Vector3 Destination { get; set; }
        

        /// <summary>
        /// On Fire
        /// </summary>
        public ActionTable OnActivated { get; } = new();
        
        /// <summary>
        /// ???
        /// </summary>
        public ActionTable OnCanceled { get; } = new();
        
        /// <summary>
        /// On Collided
        /// </summary>
        public ActionTable OnCompleted { get; } = new();
        
        /// <summary>
        /// After all
        /// </summary>
        public ActionTable OnEnded { get; } = new();


        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;
        }

        public void Active(Vector3 destination)
        {
            Destination = destination;
            
            OnActivated.Invoke();
        }


        

        protected void IsColliding()
        {
            // 충돌 처리 판별을 어떤 방식으로 할까...
            // 역시 온트리거 엔터가 제일 좋을까
            // 히트스캔, TrapComponent 방식은 이 부분이 필요없다.
        }

        protected void Cancel()
        {
            OnCanceled.Invoke();

            End();
        }

        protected void Complete()
        {
            OnCompleted.Invoke();
            
            End();
        }

        protected void End()
        {
            TrajectoryTweener = null;
            OnEnded.Invoke();
            Pool.Release(this);
        }
    }
}
