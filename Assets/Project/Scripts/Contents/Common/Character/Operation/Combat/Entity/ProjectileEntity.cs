using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class ProjectileEntity : BaseEntity
    {
        [SerializeField] private string projectileName;
        [SerializeField] private GameObject projectilePrefab;

        public string ProjectileName { get => projectileName; set => projectileName = value; }
        public override bool IsReady => true;

        public void Fire(ICombatTaker taker)
        {
            // Pooling.Draw (or spawn whatever...)
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
