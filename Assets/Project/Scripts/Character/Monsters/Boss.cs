using Common;
using Common.Actions;
using Common.Animation;
using Common.Systems;
using UnityEngine;

namespace Character.Monsters
{
    public class Boss : MonoBehaviour, ICombatExecutor, ICharacterSystem, IEditable
    {
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected RoleType role;
        [SerializeField] protected DataIndex dataIndex;
        [SerializeField] protected CharacterStatEntry statEntry;
        [SerializeField] protected ActionBehaviour characterAction;
        
        [SerializeField] protected SearchingSystem searching;
        [SerializeField] protected CollidingSystem colliding;
        [SerializeField] protected PathfindingSystem pathfinding;
        [SerializeField] protected AnimationModel animating;
        [SerializeField] protected Transform damageSpawn;
        [SerializeField] protected Transform statusEffectHierarchy;

        public DataIndex ActionCode => dataIndex;
        public string Name => characterName;
        public RoleType Role => role;
        public IDynamicStatEntry DynamicStatEntry => statEntry ??= GetComponentInChildren<CharacterStatEntry>();
        public ICharacterBehaviour CharacterBehaviour => characterAction;
        public GameObject Object => gameObject;
        public StatTable StatTable => DynamicStatEntry.StatTable;
        
        public Transform DamageSpawn => damageSpawn;
        public Transform StatusEffectHierarchy => statusEffectHierarchy;
        
        public ActionTable<CombatEntity> OnProvideDamage { get; } = new();
        public ActionTable<CombatEntity> OnProvideHeal { get; } = new();
        public ActionTable<StatusEffectEntity> OnProvideDeBuff { get; } = new();
        public ActionTable<StatusEffectEntity> OnProvideBuff { get; } = new();
        
        public ActionTable<CombatEntity> OnTakeDamage { get; } = new();
        public ActionTable<CombatEntity> OnTakeHeal { get; } = new();
        public ActionTable<StatusEffectEntity> OnTakeDeBuff { get; } = new();
        public ActionTable<StatusEffectEntity> OnTakeBuff { get; } = new();
        
        public SearchingSystem Searching => searching;
        public CollidingSystem Colliding => colliding;
        public PathfindingSystem Pathfinding => pathfinding;
        public AnimationModel Animating => animating;

        public CombatEntity TakeDamage(ICombatTable combatTable) => CombatUtility.TakeDamage(combatTable, this);
        public CombatEntity TakeHeal(ICombatTable combatTable) => CombatUtility.TakeHeal(combatTable, this);
        public StatusEffectEntity TakeDeBuff(IStatusEffect statusEffect) => CombatUtility.TakeDeBuff(statusEffect, this);
        public StatusEffectEntity TakeBuff(IStatusEffect statusEffect) => CombatUtility.TakeBuff(statusEffect, this);

        protected void Awake()
        {
            statEntry.Initialize();
        }

        private void Update() { Animating.Flip(transform.forward); }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            statEntry       ??= GetComponent<CharacterStatEntry>();
            characterAction ??= GetComponentInChildren<ActionBehaviour>();
            searching       ??= GetComponentInChildren<SearchingSystem>();
            colliding       ??= GetComponentInChildren<CollidingSystem>();
            pathfinding     ??= GetComponentInChildren<PathfindingSystem>();
            animating       ??= GetComponentInChildren<AnimationModel>();
        }
#endif
    }
}
