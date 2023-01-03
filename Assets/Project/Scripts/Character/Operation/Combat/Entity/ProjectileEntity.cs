using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class ProjectileEntity : BaseEntity
    {
        [SerializeField] private IDCode projectileID;
        [SerializeField] private GameObject projectilePrefab;

        public IDCode ProjectileID { get => projectileID; set => projectileID = value; }
        public override bool IsReady => true;

        public void Fire(ICombatTaker taker)
        {
            // Pooling.Draw (or spawn whatever...) use projectileID
            var cbPosition = Sender.Object.transform.position;
            var tkPosition = taker.Object.transform.position;
            var lookAt = Quaternion.LookRotation(tkPosition - cbPosition);
            var newProjectile = Instantiate(projectilePrefab, cbPosition, lookAt);
            //
            
            newProjectile.TryGetComponent(out ProjectileBehaviour pb);
            pb.Initialize(Sender, taker);
        }
    }
}
