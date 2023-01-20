using Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Character.Combat.Projector
{
    public abstract class ProjectorObject : CombatObject
    {
        [SerializeField] protected ProjectorShapeType shapeType;
        [SerializeField] protected Vector2 sizeValue = Vector2.zero;
        [SerializeField] protected string targetLayerType;
        
        private IObjectPool<ProjectorObject> pool; 

        public ICombatTaker Taker { get; private set; }
        public Vector3 Destination { get; private set; }
        public LayerMask TargetLayer { get; private set; }
        
        public ProjectorShapeType ShapeType => shapeType;
        public Vector2 SizeValue => sizeValue;
        public float CastingTime => CastingModule.OriginalCastingTime;

        /// <summary>
        /// 프로젝터 안에 들어와 있는 ICombatTaker 에게 Action
        /// </summary>
        public ActionTable<ICombatTaker> OnProjectorEnter { get; } = new();
        
        /// <summary>
        /// 프로젝터가 끝날 때, 안에 있는 ICombatTaker 에게 Action
        /// </summary>
        public ActionTable<ICombatTaker> OnProjectorEnd { get; } = new();
        

        public void Projection(ICombatProvider provider, ICombatTaker taker)
        {
            CoreProjection(provider);
            
            Taker       = taker;
            Destination = Taker.Object.transform.position;

            Active();
        }
        
        public void Projection(ICombatProvider provider, Vector3 destination)
        {
            CoreProjection(provider);
          
            Destination = destination;
            Active();
        }

        public void SetPool(IObjectPool<ProjectorObject> pool) => this.pool = pool;


        protected abstract void EnterProjector(ICombatTaker taker);
        protected abstract void EndProjector(ICombatTaker taker);
        protected void End()
        {
            pool.Release(this);
        }

        private void CoreProjection(ICombatProvider provider)
        {
            Provider    = provider;
            TargetLayer = CharacterUtility.SetLayer(provider, targetLayerType);
            
            if (!gameObject.activeSelf) 
                gameObject.SetActive(true);
        }

        protected void Awake()
        {
            /* ActionTable Register */
            OnCompleted.Register(InstanceID, End);
            OnProjectorEnter.Register(InstanceID, EnterProjector);
            OnProjectorEnd.Register(InstanceID, EndProjector);
        }


#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();

            var data = MainGame.MainData.GetProjector(actionCode);
            
            shapeType       =   data.ShapeType.ToEnum<ProjectorShapeType>();
            targetLayerType =   data.TargetLayerType;
            sizeValue       =   data.Size;

            GetComponents<CombatModule>().ForEach(x => ModuleUtility.SetProjectorModule(data, x));
            gameObject.GetComponentsInOnlyChildren<IInspectorSetUp>().ForEach(x => x.SetUp());
        }
#endif
    }
}
