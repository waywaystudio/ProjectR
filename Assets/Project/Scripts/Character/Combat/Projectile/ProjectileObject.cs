using System.Collections.Generic;
using Character.Combat.Entities;
using Core;
using DG.Tweening;
using MainGame;
using UnityEngine;

namespace Character.Combat.Projectile
{
    public abstract class ProjectileObject : MonoBehaviour, IActionSender, IEditorSetUp
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected float speed = 20f;
        [SerializeField] protected GameObject particle;
        [SerializeField] protected LayerMask targetLayer;

        public DataIndex ActionCode => actionCode;
        public ICombatProvider Provider { get; set; }

        protected ICombatTaker Taker;
        protected LayerMask TargetLayer => targetLayer;
        protected Dictionary<EntityType, BaseEntity> EntityTable { get; } = new();
        protected Tweener TrajectoryTweener;
        protected bool ValidateTaker => Taker != null && !Taker.Object.IsNullOrEmpty() && Taker.DynamicStatEntry.IsAlive.Value;
        private Vector3 destination;

        protected DamageEntity DamageEntity => EntityTable[EntityType.Damage] as DamageEntity;
        protected HealEntity HealEntity => EntityTable[EntityType.Heal] as HealEntity;
        protected StatusEffectEntity StatusEffectEntity => EntityTable[EntityType.StatusEffect] as StatusEffectEntity;
        
        protected Vector3 Destination
        {
            get
            {
                if (ValidateTaker) destination = Taker.Object.transform.position;
                return destination;
            }
            set => destination = value;
        }
        

        public void Initialize(ICombatProvider sender, ICombatTaker taker)
        {
            Provider = sender;
            Taker = taker;
            Destination = taker.Object.transform.position;
            EntityTable.ForEach(x => x.Value.Initialize(this));

            Trajectory();
        }

        protected abstract void Trajectory();
        protected virtual void Awake()
        {
            GetComponentsInChildren<BaseEntity>().ForEach(x => EntityTable.Add(x.Flag, x));
        }

#if UNITY_EDITOR
        public void SetUp()
        {
            if (actionCode == DataIndex.None)
                actionCode = name.ToEnum<DataIndex>();

            var projectileData = MainData.GetProjectile(actionCode);
            speed = projectileData.Speed;

            GetComponents<BaseEntity>().ForEach(x => EntityUtility.SetProjectileEntity(projectileData, x));
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