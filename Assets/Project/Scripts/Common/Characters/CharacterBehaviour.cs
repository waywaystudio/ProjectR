using Common.Actions;
using Common.Animation;
using Common.Characters.Behaviours;
using Common.Characters.Behaviours.CrowdControlEffect;
using Common.Characters.Behaviours.Movement;
using Common.Systems;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterBehaviour : MonoBehaviour, ICombatExecutor, ICharacterSystem, ICharacterAnimation, IEditable
    {
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected DataIndex characterID;
        [SerializeField] protected RoleType role;
        [SerializeField] protected CharacterStatEntry statEntry;
        [SerializeField] protected SearchingSystem searching;
        [SerializeField] protected CollidingSystem colliding;
        [SerializeField] protected PathfindingSystem pathfinding;
        [SerializeField] protected AnimationModel animating;
        [SerializeField] protected Transform damageSpawn;
        [SerializeField] protected Transform statusEffectHierarchy;
        
        // TODO. : Now on CharacterBehaviour Working...
        [SerializeField] protected OldActionBehaviour characterAction;

        [SerializeField] protected StopBehaviour stopBehaviour;
        [SerializeField] protected RunBehaviour runBehaviour;
        [SerializeField] protected StunBehaviour stunBehaviour;
        [SerializeField] protected KnockBackBehaviour knockBackBehaviour;
        [SerializeField] protected DeadBehaviour deadBehaviour;
        
        public StopBehaviour StopBehaviour => stopBehaviour;
        public RunBehaviour RunBehaviour => runBehaviour;
        public StunBehaviour StunBehaviour => stunBehaviour;
        public KnockBackBehaviour KnockBackBehaviour => knockBackBehaviour;
        public DeadBehaviour DeadBehaviour => deadBehaviour;

        public CharacterActionMask BehaviourMask => CurrentBehaviour is null ? CharacterActionMask.None : CurrentBehaviour.BehaviourMask;
        public ActionBehaviour CurrentBehaviour { get; set; }

        public void Rotate(Vector3 lookTarget) { Pathfinding.RotateToTarget(lookTarget); Animating.Flip(transform.forward); }
        public void Stop() => stopBehaviour.Active();
        public void Run(Vector3 destination) => runBehaviour.Active(destination);
        public void Stun(float duration) => stunBehaviour.Active(duration);
        public void KnockBack(Vector3 source, float distance) => knockBackBehaviour.Active(source, distance);
        public void Dead() => deadBehaviour.Dead();

        public DataIndex ActionCode => characterID;
        public RoleType Role => role;
        public string Name => characterName;
        public IDynamicStatEntry DynamicStatEntry => statEntry ??= GetComponentInChildren<CharacterStatEntry>();
        public IActionBehaviour ActionBehaviour => characterAction;
        public GameObject Object => gameObject;
        public StatTable StatTable => DynamicStatEntry.StatTable;
        
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
        
        public Transform DamageSpawn => damageSpawn;
        public Transform StatusEffectHierarchy => statusEffectHierarchy;
        
        public OldActionBehaviour CharacterAction => characterAction;
        


        public CombatEntity TakeDamage(ICombatTable combatTable) => CombatUtility.TakeDamage(combatTable, this);
        public CombatEntity TakeHeal(ICombatTable combatTable) => CombatUtility.TakeHeal(combatTable, this);
        public StatusEffectEntity TakeDeBuff(IStatusEffect statusEffect) => CombatUtility.TakeDeBuff(statusEffect, this);
        public StatusEffectEntity TakeBuff(IStatusEffect statusEffect) => CombatUtility.TakeBuff(statusEffect, this);

        
#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            statEntry          ??= GetComponent<CharacterStatEntry>();
            characterAction    ??= GetComponentInChildren<OldActionBehaviour>();
            stopBehaviour       ??= GetComponentInChildren<StopBehaviour>();
            runBehaviour       ??= GetComponentInChildren<RunBehaviour>();
            stunBehaviour      ??= GetComponentInChildren<StunBehaviour>();
            knockBackBehaviour ??= GetComponentInChildren<KnockBackBehaviour>();
            deadBehaviour      ??= GetComponentInChildren<DeadBehaviour>();

            searching       ??= GetComponentInChildren<SearchingSystem>();
            colliding       ??= GetComponentInChildren<CollidingSystem>();
            pathfinding     ??= GetComponentInChildren<PathfindingSystem>();
            animating       ??= GetComponentInChildren<AnimationModel>();
        }
#endif
    }
}
