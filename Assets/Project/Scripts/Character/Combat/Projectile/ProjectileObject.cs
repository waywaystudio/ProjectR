using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Character.Combat.Projectile
{
    public abstract class ProjectileObject : CombatObject
    {
        [SerializeField] protected float speed = 20f;
        [SerializeField] protected LayerMask targetLayer;

        protected ICombatTaker Taker;
        protected LayerMask TargetLayer => targetLayer;
        protected Tweener TrajectoryTweener;
        protected bool ValidateTaker => Taker != null && !Taker.Object.IsNullOrEmpty() && Taker.DynamicStatEntry.IsAlive.Value;
        private Vector3 destination;
        private IObjectPool<ProjectileObject> projectilePool;

        protected Vector3 Destination
        {
            get
            {
                if (ValidateTaker) destination = Taker.Object.transform.position;
                return destination;
            }
            set => destination = value;
        }
        

        public void Initialize(ICombatProvider provider, ICombatTaker taker)
        {
            Provider    = provider;
            Taker       = taker;
            Destination = taker.Object.transform.position;

            Active();
        }
        
        public override void Complete()
        {
            base.Complete();
            
            TrajectoryTweener = null;
            projectilePool.Release(this);
        }

        public void SetPool(IObjectPool<ProjectileObject> pool) => projectilePool = pool;
        

        protected abstract void Trajectory();
        protected abstract void Arrived();
        protected virtual void Awake()
        {
            OnActivated.Register(InstanceID, Trajectory);
            OnCompleted.Register(InstanceID, Arrived);
        }


#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();

            var projectileData = MainGame.MainData.GetProjectile(actionCode);
            speed = projectileData.Speed;

            GetComponents<CombatModule>().ForEach(x => ModuleUtility.SetProjectileModule(projectileData, x));
        }
#endif
    }
}

/*
 * Annotation
 * tween.onComplete += 는 Awake에서 안들어온다. 아마도 DoTween Awake보다 늦게 들어와서 그런듯 하다.
 * 수동으로 DoTween.Init()을 해줘도 문제가 발생한다. 괜히 건드리지 말자.
 * 재활용하는 경우 문제가 생길 수도 있을 것 같다. 람다 타입으로 들어가니, 해제는 힘들고 onComplete 초기화를 해줘야 할 것 같다.
 */