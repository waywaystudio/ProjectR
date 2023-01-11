using Character.Combat.Skill.Modules;
using Core;
using DG.Tweening;
using MainGame;
using UnityEngine;

namespace Character.Combat.Projectile
{
    public abstract class ProjectileObject : CombatObject, IEditorSetUp
    {
        [SerializeField] protected float speed = 20f;
        [SerializeField] protected LayerMask targetLayer;

        protected ICombatTaker Taker;
        protected LayerMask TargetLayer => targetLayer;
        protected Tweener TrajectoryTweener;
        protected bool ValidateTaker => Taker != null && !Taker.Object.IsNullOrEmpty() && Taker.DynamicStatEntry.IsAlive.Value;
        private Vector3 destination;

        protected DamageSkill DamageEntity => GetModule<DamageSkill>(ModuleType.Damage);
        protected HealSkill HealEntity => GetModule<HealSkill>(ModuleType.Heal);
        protected StatusEffectSkill StatusEffectEntity => GetModule<StatusEffectSkill>(ModuleType.StatusEffect);
        
        protected Vector3 Destination
        {
            get
            {
                if (ValidateTaker) destination = Taker.Object.transform.position;
                return destination;
            }
            set => destination = value;
        }
        

        public override void Initialize(ICombatProvider provider, ICombatTaker taker)
        {
            Provider = provider;
            Taker = taker;
            Destination = taker.Object.transform.position;

            Trajectory();
        }

        protected abstract void Trajectory();
 

#if UNITY_EDITOR
        public void SetUp()
        {
            if (actionCode == DataIndex.None)
                actionCode = name.ToEnum<DataIndex>();

            var projectileData = MainData.GetProjectile(actionCode);
            speed = projectileData.Speed;

            GetComponents<SkillModule>().ForEach(x => EntityUtility.SetProjectileEntity(projectileData, x));
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