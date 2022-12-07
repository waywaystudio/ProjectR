using Common.Character.Skills.Core;
using Core;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class ProjectileEntity : EntityAttribution
    {
        // TODO. Projectile Master같은 곳에서 키 값으로 가져오고, SerializeField 띄어내볼까...
        [SerializeField] private GameObject projectile;
        private IDamageProvider provider;
        private ICombatTaker taker;
        
        public override bool IsReady => true;

        public void Initialize(IDamageProvider provider, ICombatTaker taker)
        {
            // SetEntity...
        }

        public void Fire()
        {
            // projectile.Instantiate or Draw(Spawn...Whatever);
            // projectile.SetDestination(destination)
        }
        
        protected override void SetEntity()
        {
            // projectile = MainData.TryGetProjectile(projectileName...);
        }


        private void OnEnable()
        {
            Skill.OnCompleted += Fire;
        }

        private void OnDisable()
        {
            Skill.OnCompleted -= Fire;
        }

#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            Flag = EntityType.Projectile;
            
            SetEntity();
        }
#endif
    }
}
