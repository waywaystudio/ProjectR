using Common.Projectile;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    // Projectile Entity의 주요 목표는
    // 알맞은 Prefab을 찾고, 전투관련 Value를 전달 혹은 복사시키는 것.
    // 속도, 운동방법, 후처리 등은 Prefab이 결정.
    // 따라서 ProjectilePrefab.Initialize()를 어떻게 해줄 것인가가 관건.
    public class ProjectileEntity : BaseEntity
    {
        // TODO. Projectile Master같은 곳에서 키 값으로 가져오고,
        [SerializeField] private int projectileID;
        [SerializeField] private GameObject projectile;
        private ICombatTaker taker;
        
        public override bool IsReady => true;
        public ActionTable OnArrived { get; } = new();
        public ActionTable OnCollided { get; } = new();

        public void Initialize(ICombatTaker taker)
        {
            this.taker = taker;
        }

        public void Fire()
        {
            if (taker == null) return;
            
            // Pooling
            var cbPosition = Cb.transform.position;
            var tkPosition = taker.Object.transform.position;
            var lookAt = Quaternion.LookRotation(tkPosition - cbPosition);
            var newProjectile = Instantiate(projectile, cbPosition, lookAt);
            //
            
            newProjectile.TryGetComponent(out ProjectileBehaviour pb);
            pb.Initialize(taker, OnArrived, OnCollided);
        }

        public override void SetEntity()
        {
            // projectileID = SkillData.Projectile;
        }

        private void OnEnable()
        {
            AssignedSkill.OnCompleted.Register(InstanceID, Fire);
        }

        private void OnDisable()
        {
            AssignedSkill.OnCompleted.Unregister(InstanceID);
        }
        
        private void Reset()
        {
            flag = EntityType.Projectile;
            
            var skillData = MainGame.MainData.GetSkillData(GetComponent<BaseSkill>().ActionName);
            projectileID = skillData.Projectile;
        }
    }
}
