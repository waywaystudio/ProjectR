using System;
using Core;
using UnityEngine;

namespace Character.Combat
{
    using Projectile;
    
    public class ProjectileModule : CombatModule
    {
        [SerializeField] private DataIndex projectileID;

        // TODO. 이후에는, IDCode 혹은 ProjectileName을 통해서 풀링하고, GameObject Field를 삭제하자.
        [SerializeField] private GameObject projectilePrefab;

        public DataIndex ProjectileID { get => projectileID; set => projectileID = value; }

        public void Fire(ICombatTaker taker)
        {
            // Pooling.Draw (or spawn whatever...) use projectileID
            var cbPosition = Provider.Object.transform.position;
            var tkPosition = taker.Object.transform.position;
            var lookAt = Quaternion.LookRotation(tkPosition - cbPosition);
            var newProjectile = Instantiate(projectilePrefab, cbPosition, lookAt);
            //
            
            newProjectile.TryGetComponent(out ProjectileObject pb);
            pb.Initialize(Provider, taker);
        }


#if UNITY_EDITOR
        public void SetUpValue(int id)
        {
            Flag         = ModuleType.Projectile;
            projectileID = (DataIndex)id;
        }
#endif
    }
}
