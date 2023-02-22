// using Core;
// using UnityEngine;
// using UnityEngine.Pool;
//
// namespace Character.Combat
// {
//     using Projectile;
//     
//     public class ProjectileModule : OldCombatModule
//     {
//         // TODO. 이후에는, IDCode 혹은 ProjectileName을 통해서 풀링하고, GameObject Field를 삭제하자.
//         [SerializeField] private GameObject projectilePrefab;
//         [SerializeField] private DataIndex projectileID;
//         [SerializeField] private int maxPool = 8;
//
//         private ICombatTaker taker;
//         private IObjectPool<ProjectileObject> pool;
//         
//         public ICombatProvider Provider => CombatObject.Provider;
//         
//
//         public void Fire(ICombatTaker taker)
//         {
//             this.taker = taker;
//
//             pool.Get();
//         }
//         
//
//         protected ProjectileObject CreateProjectile()
//         {
//             var projectile = Instantiate(projectilePrefab).GetComponent<ProjectileObject>();
//             
//             projectile.SetPool(pool);
//
//             return projectile;
//         }
//
//         protected void OnProjectileGet(ProjectileObject projectile)
//         {
//             var cbPosition = Provider.Object.transform.position;
//             var tkPosition = taker.Object.transform.position;
//             var lookAt = Quaternion.LookRotation(tkPosition - cbPosition);
//             
//             projectile.transform.SetPositionAndRotation(cbPosition, lookAt);
//             projectile.gameObject.SetActive(true);
//             
//             projectile.Initialize(Provider, taker);
//         }
//
//         protected static void OnProjectileRelease(ProjectileObject projectile)
//         {
//             projectile.gameObject.SetActive(false);
//         }
//
//         protected static void OnProjectileDestroy(ProjectileObject projectile)
//         {
//             Destroy(projectile.gameObject);
//         }
//
//         protected override void Awake()
//         {
//             base.Awake();
//
//             pool = new ObjectPool<ProjectileObject>(
//                 CreateProjectile,
//                 OnProjectileGet,
//                 OnProjectileRelease,
//                 OnProjectileDestroy,
//                 maxSize: maxPool);
//         }
//
// #if UNITY_EDITOR
//         public void SetUpValue(int id)
//         {
//             Flag         = CombatModuleType.Projectile;
//             projectileID = (DataIndex)id;
//         }
// #endif
//     }
// }
