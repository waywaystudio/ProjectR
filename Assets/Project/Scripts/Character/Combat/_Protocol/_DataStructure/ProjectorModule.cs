using Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Character.Combat
{
    using Projector;
    
    public class ProjectorModule : OldCombatModule
    {
        // TODO. 이후에는, IDCode 혹은 ProjectileName을 통해서 풀링하고, GameObject Field를 삭제하자.
        [SerializeField] private GameObject projectorPrefab;
        [SerializeField] private DataIndex projectorID;
        [SerializeField] private int maxPool = 8;
        
        private IObjectPool<ProjectorObject> pool;
        

        public void Projection(ICombatTaker taker)
        {
            pool.Get(out var po);
            po.Projection(CombatObject.Provider, taker);
        }

        public void Projection(Vector3 position)
        {
            pool.Get(out var po);
            po.Projection(CombatObject.Provider, position);
        }
        
        
        protected ProjectorObject CreateProjector()
        {
            var projector = Instantiate(projectorPrefab).GetComponent<ProjectorObject>();
            
            projector.SetPool(pool);

            return projector;
        }

        protected void OnProjectorGet(ProjectorObject projector)
        {
            projector.gameObject.SetActive(true);
        }

        protected static void OnProjectorRelease(ProjectorObject projector)
        {
            projector.gameObject.SetActive(false);
        }

        protected static void OnProjectorDestroy(ProjectorObject projector)
        {
            Destroy(projector.gameObject);
        }
        
        protected override void Awake()
        {
            base.Awake();

            pool = new ObjectPool<ProjectorObject>(
                CreateProjector,
                OnProjectorGet,
                OnProjectorRelease,
                OnProjectorDestroy,
                maxSize: maxPool);
        }


#if UNITY_EDITOR
        public void SetUpValue(int projectorID)
        {
            Flag             = CombatModuleType.Projector;
            this.projectorID = (DataIndex)projectorID;
        }
#endif
    }
}
