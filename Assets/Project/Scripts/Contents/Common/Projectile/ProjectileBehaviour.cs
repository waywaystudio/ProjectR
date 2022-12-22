using Core;
using DG.Tweening;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Projectile
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected string projectileName;
        [SerializeField] protected float speed = 20f;
        [SerializeField] private GameObject particle;

        protected Tweener TrajectoryTweener;
        protected Vector3 destination;

        public int ID { get; set; }
        public string Name { get => projectileName; set => projectileName = value; }
        public ICombatTaker Taker { get; set; }
        public LayerMask TargetLayer => targetLayer;
        [ShowInInspector]
        public ActionTable OnArrived { get; set; }
        [ShowInInspector]
        public ActionTable OnCollided { get; set; }

        public virtual Vector3 Destination
        {
            get
            {
                if (Taker != null && !Taker.Object.IsNullOrEmpty() && Taker.IsAlive)
                {
                    destination = Taker.Object.transform.position;
                }

                return destination;
            }
            set => destination = value;
        }

        public virtual void Initialize(ICombatTaker taker, ActionTable completeAction, ActionTable collidedAction)
        {
            Taker = taker;
            Destination = taker.Object.transform.position;
            OnArrived = completeAction;
            OnCollided = collidedAction;

            // Trajectory do first then onComplete +
            Trajectory();

            TrajectoryTweener.onComplete = null;
            TrajectoryTweener.onComplete += completeAction.Invoke;

            // Set onComplete Action;
            // identity Action (ex. Chain)
        }

        public virtual void Trajectory()
        {
            TrajectoryTweener = transform
                .DOMove(Destination, speed)
                .SetEase(Ease.Linear)
                .SetSpeedBased();

            TrajectoryTweener.OnUpdate(() =>
            {
                var takerPosition = Taker.Object.transform.position;
                
                if (Vector3.Distance(transform.position, takerPosition) > 1f)
                {
                    TrajectoryTweener
                        .ChangeEndValue(takerPosition, speed, true)
                        .SetSpeedBased();
                }
            });
        }

#if UNITY_EDITOR
        [Button]
        private void GetEditorProjectileFromDB()
        {
            var data = MainData.GetProjectileData(Name);
            ID = data.ID;
            speed = data.Speed;
            // particle = data.Particle
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