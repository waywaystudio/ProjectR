using Core;
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
        // SerializeField 띄어내볼까...
        [SerializeField] private GameObject projectile;
        private ICombatProvider provider;
        private ICombatTaker taker;
        
        public override bool IsReady => true;

        public void Initialize(ICombatProvider provider, ICombatTaker taker)
        {
            // SetEntity...
        }

        public void Fire()
        {
            Debug.Log("Projectile Fire!");
            // projectile.Instantiate or Draw(Spawn...Whatever);
            // projectile.SetDestination(destination)
        }

        public override void SetEntity()
        {
            
        }

        private void OnEnable()
        {
            Skill.OnCompleted.Register(InstanceID, Fire);
        }

        private void OnDisable()
        {
            Skill.OnCompleted.Unregister(InstanceID);
        }

        private void Reset()
        {
            flag = EntityType.Projectile;
        }
    }
}
