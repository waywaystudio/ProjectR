using Core;
using UnityEngine;

namespace Character.Combat.Projector
{
    public abstract class ProjectorObject : CombatObject
    {
        // for Test
        public CharacterBehaviour TempProvider;
        public CharacterBehaviour TempTarget;

        [SerializeField] protected ProjectorShapeType shapeType;
        [SerializeField] protected Vector2 sizeValue = Vector2.zero;
        [SerializeField] protected string targetLayerType;

        public ICombatTaker Taker { get; private set; }
        public Vector3 Destination { get; set; }
        public ProjectorShapeType ShapeType => shapeType;
        public Vector2 SizeValue => sizeValue;
        public float CastingTime => CastingModule.OriginalCastingTime;
        public LayerMask TargetLayer { get; set; }
        
        /// <summary>
        /// Call From ProjectorModule.Projection
        /// </summary>
        public ActionTable OnProjectionStart { get; } = new();
        
        /// <summary>
        /// Call By ProjectorDecal Shader Progression.
        /// </summary>
        public ActionTable OnProjectionEnd { get; } = new();
        
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

            OnProjectionStart.Invoke();
        }
        
        public void Projection(ICombatProvider provider, Vector3 destination)
        {
            CoreProjection(provider);
          
            Destination = destination;
            
            OnProjectionStart.Invoke();
        }


        protected abstract void EnterProjector(ICombatTaker taker);
        protected abstract void EndProjector(ICombatTaker taker);

        protected void End()
        {
            // Return to Pool
            Destroy(gameObject);
        }

        private void CoreProjection(ICombatProvider provider)
        {
            Provider    = provider;
            TargetLayer = CharacterUtility.SetLayer(provider, targetLayerType);
            
            ModuleTable.ForEach(x => x.Value.Initialize(this));
            
            if (!gameObject.activeSelf) 
                gameObject.SetActive(true);
        }

        protected override void Awake()
        {
            base.Awake();

            /* ActionTable Register */
            OnProjectionEnd.Register(InstanceID, End);
            OnProjectorEnter.Register(InstanceID, EnterProjector);
            OnProjectorEnd.Register(InstanceID, EndProjector);
        }


#if UNITY_EDITOR
        public override void SetUp()
        {
            if (actionCode == DataIndex.None) 
                actionCode = GetType().Name.ToEnum<DataIndex>();

            var data = MainGame.MainData.GetProjector(actionCode);
            
            shapeType       =   data.ShapeType.ToEnum<ProjectorShapeType>();
            targetLayerType =   data.TargetLayerType;
            sizeValue       =   data.Size;

            GetComponents<Module>().ForEach(x => ModuleUtility.SetProjectorModule(data, x));
            gameObject.GetComponentsInOnlyChildren<IEditorSetUp>().ForEach(x => x.SetUp());
        }
#endif
    }
}
