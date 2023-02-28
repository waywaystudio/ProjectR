using Character.Actions;
using Character.Data;
using Character.Data.BaseStats;
using Character.Graphic;
using Character.Systems;
using Core;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class Adventurer : MonoBehaviour, ICombatExecutor, ICharacterSystem
    {
        public bool IsAutoStart;
        
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected RoleType role;
        [SerializeField] protected DataIndex combatClassID;
        [SerializeField] protected DynamicStatEntry dynamicStatEntry;
        [SerializeField] protected CharacterConstStats constStats;
        [SerializeField] protected ActionBehaviour actionBehaviour;
        [SerializeField] protected SearchingSystem searching;
        [SerializeField] protected CollidingSystem colliding;
        [SerializeField] protected PathfindingSystem pathfinding;
        [SerializeField] protected AnimationModel animating;
        [SerializeField] protected UnityEvent onManualControl;
        [SerializeField] protected UnityEvent onAutoControl;
        
        [SerializeField] protected Transform damageSpawn;
        [SerializeField] protected Transform statusEffectHierarchy;

        public ActionBehaviour ActionBehaviour => actionBehaviour;
        public Transform DamageSpawn => damageSpawn;
        public Transform StatusEffectHierarchy => statusEffectHierarchy;

        private bool IsAutoMode { get; set; }

        public string Name => characterName;
        public RoleType Role => role;
        public DataIndex ClassCode => combatClassID;
        public IDynamicStatEntry DynamicStatEntry => dynamicStatEntry ??= GetComponentInChildren<DynamicStatEntry>();
        public GameObject Object => gameObject;
        public StatTable StatTable => DynamicStatEntry.StatTable;
        public ActionTable OnDead { get; } = new();

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

        public void Dead() => OnDead.Invoke();
        public CombatEntity TakeDamage(ICombatTable combatTable) => CombatUtility.TakeDamage(combatTable, this);
        public CombatEntity TakeHeal(ICombatTable combatTable) => CombatUtility.TakeHeal(combatTable, this);
        public StatusEffectEntity TakeDeBuff(IStatusEffect statusEffect) => CombatUtility.TakeDeBuff(statusEffect, this);
        public StatusEffectEntity TakeBuff(IStatusEffect statusEffect) => CombatUtility.TakeBuff(statusEffect, this);


        public void OnFocused(Adventurer focus)
        {
            if (focus == this)
            {
                IsAutoMode = false;
                onManualControl.Invoke();
            }
            else
            {
                onAutoControl.Invoke();
            }
        }
        
        protected void Awake()
        {
            constStats       ??= GetComponentInChildren<CharacterConstStats>();
            dynamicStatEntry ??= GetComponentInChildren<DynamicStatEntry>();
            actionBehaviour  ??= GetComponentInChildren<ActionBehaviour>();
            searching        ??= GetComponentInChildren<SearchingSystem>();
            colliding        ??= GetComponentInChildren<CollidingSystem>();
            pathfinding      ??= GetComponentInChildren<PathfindingSystem>();
            animating        ??= GetComponentInChildren<AnimationModel>();

            constStats.Initialize(StatTable);
            dynamicStatEntry.Initialize();
            OnDead.Register("DynamicAliveValue", () => dynamicStatEntry.IsAlive.Value = false);

            IsAutoMode = IsAutoStart;
            
            if (IsAutoMode) onAutoControl.Invoke();
            else
            {
                onManualControl.Invoke();
            }
        }
    }
}