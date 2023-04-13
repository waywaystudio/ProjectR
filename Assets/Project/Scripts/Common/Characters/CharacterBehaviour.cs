using System;
using Common.Animation;
using Common.Characters.Behaviours;
using Common.Characters.Behaviours.CrowdControlEffect;
using Common.Characters.Behaviours.Movement;
using Common.Skills;
using Common.Systems;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterBehaviour : MonoBehaviour, ICombatExecutor, ICharacterSystem, ICharacterAnimation, IEditable
    {
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected DataIndex characterID;
        [SerializeField] protected CombatClassType role;
        
        [SerializeField] protected CharacterStats stats;
        [SerializeField] protected CharacterEquipment equipment;
        
        [SerializeField] protected AnimationModel animating;
        [SerializeField] protected Transform damageSpawn;
        [SerializeField] protected Transform statusEffectHierarchy;
        
        [SerializeField] protected StopBehaviour stopBehaviour;
        [SerializeField] protected RunBehaviour runBehaviour;
        [SerializeField] protected StunBehaviour stunBehaviour;
        [SerializeField] protected KnockBackBehaviour knockBackBehaviour;
        [SerializeField] protected DeadBehaviour deadBehaviour;
        [SerializeField] protected SkillBehaviour skillBehaviour;
        
        [SerializeField] protected SearchingSystem searching;
        [SerializeField] protected CollidingSystem colliding;
        [SerializeField] protected PathfindingSystem pathfinding;

        public bool IsInitialized { get; protected set; } = false;

        public CharacterActionMask BehaviourMask => CurrentBehaviour is null ? CharacterActionMask.None : CurrentBehaviour.BehaviourMask;
        public ActionBehaviour CurrentBehaviour { get; set; }
        public DataIndex ActionCode => characterID;
        public CombatClassType CombatClass => role;
        public string Name => characterName;
        public IDynamicStatEntry DynamicStatEntry => stats ??= GetComponent<CharacterStats>();
        public StatTable StatTable => DynamicStatEntry.StatTable;
        public Vector3 Position => transform.position;
        public CharacterEquipment Equipment => equipment;

        public SearchingSystem Searching => searching;
        public CollidingSystem Colliding => colliding;
        public PathfindingSystem Pathfinding => pathfinding;
        public AnimationModel Animating => animating;
        
        public Transform DamageSpawn => damageSpawn;
        public Transform StatusEffectHierarchy => statusEffectHierarchy;

        public SkillComponent GetSkill(DataIndex actionCode) => skillBehaviour.GetSkill(actionCode);
        public StopBehaviour StopBehaviour => stopBehaviour;
        public RunBehaviour RunBehaviour  => runBehaviour;
        public StunBehaviour StunBehaviour => stunBehaviour;
        public KnockBackBehaviour KnockBackBehaviour => knockBackBehaviour;
        public DeadBehaviour DeadBehaviour => deadBehaviour;
        public SkillBehaviour SkillBehaviour => skillBehaviour;
        
        public void Rotate(Vector3 lookTarget) { Pathfinding.RotateToTarget(lookTarget); Animating.Flip(transform.forward); }
        public void Stop() => stopBehaviour.Active();
        public void Run(Vector3 destination) => runBehaviour.Active(destination);
        public void Stun(float duration) => stunBehaviour.Active(duration);
        public void KnockBack(Vector3 source, float distance) => knockBackBehaviour.Active(source, distance);
        public void Dead() => deadBehaviour.Dead();
        public void ExecuteSkill(DataIndex actionCode, Vector3 targetPosition) => skillBehaviour.Active(actionCode, targetPosition);
        public void CancelSkill() => skillBehaviour.Cancel();
        public void ReleaseSkill() => skillBehaviour.Release();

        public ActionTable<CombatEntity> OnDamageProvided { get; } = new();
        public ActionTable<CombatEntity> OnDamageTaken { get; } = new();
        public ActionTable<CombatEntity> OnHealProvided { get; } = new();
        public ActionTable<CombatEntity> OnHealTaken { get; } = new();
        public ActionTable<IStatusEffect> OnDeBuffProvided { get; } = new();
        public ActionTable<IStatusEffect> OnDeBuffTaken { get; } = new();
        public ActionTable<IStatusEffect> OnBuffProvided { get; } = new();
        public ActionTable<IStatusEffect> OnBuffTaken { get; } = new();


        public virtual void Initialize() { }

        
        private void OnEnable()
        {
            OnDeBuffTaken.Register("RegisterTable", stats.DeBuffTable.Register);
            OnBuffTaken.Register("RegisterTable", stats.BuffTable.Register);
        }

        private void OnDisable()
        {
            OnDeBuffTaken.Unregister("RegisterTable");
            OnBuffTaken.Unregister("RegisterTable");
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            stats          ??= GetComponent<CharacterStats>();
            
            equipment          ??= GetComponentInChildren<CharacterEquipment>(); 
            skillBehaviour     ??= GetComponentInChildren<SkillBehaviour>();
            stopBehaviour      ??= GetComponentInChildren<StopBehaviour>();
            runBehaviour       ??= GetComponentInChildren<RunBehaviour>();
            stunBehaviour      ??= GetComponentInChildren<StunBehaviour>();
            knockBackBehaviour ??= GetComponentInChildren<KnockBackBehaviour>();
            deadBehaviour      ??= GetComponentInChildren<DeadBehaviour>();
            searching          ??= GetComponentInChildren<SearchingSystem>();
            colliding          ??= GetComponentInChildren<CollidingSystem>();
            pathfinding        ??= GetComponentInChildren<PathfindingSystem>();
            animating          ??= GetComponentInChildren<AnimationModel>();
        }
#endif
    }
}
