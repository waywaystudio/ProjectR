using Common.Projectile;
using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class ProjectileEntity : BaseEntity
    {
        [SerializeField] private GameObject projectilePrefab;

        public override bool IsReady => true;

        public void Fire(ICombatProvider provider, ICombatTaker taker)
        {
            Sender = provider;

            // Pooling.Draw (or spawn whatever...)
            var cbPosition = Sender.Object.transform.position;
            var tkPosition = taker.Object.transform.position;
            var lookAt = Quaternion.LookRotation(tkPosition - cbPosition);
            var newProjectile = Instantiate(projectilePrefab, cbPosition, lookAt);
            //
            
            newProjectile.TryGetComponent(out ProjectileBehaviour pb);
            pb.Initialize(Sender, taker);
        }

        public override void SetEntity() {}
        private void Reset()
        {
            flag = EntityType.Projectile;

            SetEntity();
        }
    }
}
