using Character.Combat.Projectile;
using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    public class ProjectileEntity : BaseEntity
    {
        [SerializeField] private DataIndex projectileID;
        
        // TODO. 이후에는, IDCode 혹은 ProjectileName을 통해서 풀링하고, GameObject Field를 삭제하자.
        [SerializeField] private GameObject projectilePrefab;

        public DataIndex ProjectileID { get => projectileID; set => projectileID = value; }
        public override bool IsReady => true;

        public void Fire(ICombatTaker taker)
        {
            // Pooling.Draw (or spawn whatever...) use projectileID
            var cbPosition = Provider.Object.transform.position;
            var tkPosition = taker.Object.transform.position;
            var lookAt = Quaternion.LookRotation(tkPosition - cbPosition);
            var newProjectile = Instantiate(projectilePrefab, cbPosition, lookAt);
            //
            
            newProjectile.TryGetComponent(out ProjectileBehaviour pb);
            pb.Initialize(Provider, taker);
        }
        
        
#if UNITY_EDITOR
        public void SetUpValue(int projectileID)
        {
            ProjectileID = (DataIndex)projectileID;
            Flag         = EntityType.Projectile;
        }
#endif
    }
}
