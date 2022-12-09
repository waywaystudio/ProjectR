using Core;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public class ProjectileEntity : BaseEntity
    {
        // TODO. Projectile Master같은 곳에서 키 값으로 가져오고,
        // SerializeField 띄어내볼까...
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
            Debug.Log("Projectile Fire!");
            // projectile.Instantiate or Draw(Spawn...Whatever);
            // projectile.SetDestination(destination)
        }

        private void OnEnable()
        {
            Skill.OnCompleted += Fire;
        }

        private void OnDisable()
        {
            Skill.OnCompleted -= Fire;
        }

        private void Reset()
        {
            flag = EntityType.Projectile;
        }
    }
}
