using System.Collections.Generic;
using Core;
using DG.Tweening;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combat
{
    using Entity;
    
    public abstract class ProjectileBehaviour : MonoBehaviour, IActionSender
    {
        [SerializeField] protected int id;
        [SerializeField] protected string projectileName;
        [SerializeField] protected float speed = 20f;
        [SerializeField] protected GameObject particle;
        [SerializeField] protected LayerMask targetLayer;

        public string ActionName => projectileName;
        public ICombatProvider Sender { get; set; }
        protected ICombatTaker Taker { get; set; }
        protected LayerMask TargetLayer => targetLayer;
        protected Dictionary<EntityType, BaseEntity> EntityTable { get; } = new();
        protected Tweener TrajectoryTweener;
        protected bool ValidateTaker => Taker != null && !Taker.Object.IsNullOrEmpty() && Taker.IsAlive;
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
            Sender = sender;
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
        [PropertySpace(15f, 0f)]
        [Button(ButtonSizes.Large,  Icon = SdfIconType.ArrowRepeat, Stretch = false)]
        private void GetProjectileFromDB()
        {
            var data = MainData.GetProjectileData(projectileName);
            id = data.ID;
            speed = data.Speed;
            
            if (TryGetComponent(out DamageEntity damageEntity))
            {
                damageEntity.DamageValue = data.BaseValue;
                damageEntity.Flag = EntityType.Damage;
            }
            if (TryGetComponent(out CastingEntity castingEntity))
            {
                // castingEntity.OriginalCastingTime = data.CastingTime;
                castingEntity.Flag = EntityType.Casting;
            }
            if (TryGetComponent(out CoolTimeEntity coolTimeEntity))
            {
                // coolTimeEntity.CoolTime = data.BaseCoolTime;
                coolTimeEntity.Flag = EntityType.CoolTime;
            }
            if (TryGetComponent(out HealEntity healEntity))
            {
                healEntity.HealValue = data.BaseValue;
                healEntity.Flag = EntityType.Heal;
            }
            if (TryGetComponent(out ProjectileEntity projectileEntity))
            {
                // projectileEntity.ProjectileName = data.Projectile;
                projectileEntity.Flag = EntityType.Projectile;
            }
            if (TryGetComponent(out StatusEffectEntity statusEffectEntity))
            {
                statusEffectEntity.ActionName = data.StatusEffect;
                statusEffectEntity.Flag = EntityType.StatusEffect;
            }
            if (TryGetComponent(out TargetEntity targetEntity))
            {
                // targetEntity.TargetLayerType = data.TargetLayer;
                // targetEntity.TargetCount = data.TargetCount;
                // targetEntity.Range = data.Range;
                targetEntity.Flag = EntityType.Target;
            }
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