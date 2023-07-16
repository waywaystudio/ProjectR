using System.Collections.Generic;
using Common.Animation;
using Common.StatusEffects;
using Common.Systems;
using Common.TargetSystem;
using UnityEngine;

namespace Common.Characters
{
    using Behaviours;

    public class CharacterBehaviour : MonoBehaviour, ICombatExecutor, ICharacterSystem, ISearchable, IEditable
    {
        [SerializeField] protected CharacterCombatStatus combatStatus;
        [SerializeField] protected CharacterPreposition prePosition;
        [SerializeField] protected AnimationModel animating;
        [SerializeField] protected SearchEngine searchEngine;
        [SerializeField] protected PathfindingSystem pathfinding;
        
        [SerializeField] protected StopBehaviour stopBehaviour;
        [SerializeField] protected RunBehaviour runBehaviour;
        [SerializeField] protected StunBehaviour stunBehaviour;
        [SerializeField] protected KnockBackBehaviour knockBackBehaviour;
        [SerializeField] protected DrawBehaviour drawBehaviour;
        [SerializeField] protected DeadBehaviour deadBehaviour;
        [SerializeField] protected SkillTable skillBehaviour;
        [SerializeField] protected Transform statusEffectHierarchy;

        /*
         * Common Attribute
         */
        public virtual DataIndex DataIndex => DataIndex.None;
        public virtual CharacterMask CombatClass => Data.CharacterMask;
        public virtual string Name => "characterName";
        public virtual CharacterData Data { get; set; }
        public Vector3 Position => transform.position;
        public Vector3 Forward => transform.forward;


        /*
         * Systems
         */
        public Dictionary<int, List<GameObject>> SearchedTable => searchEngine.SearchedTable;
        public PathfindingSystem Pathfinding => pathfinding;
        public AnimationModel Animating => animating;
        

        /*
         * Behaviour Attribute
         */
        public ActionMask BehaviourMask => CurrentBehaviour is null ? ActionMask.None : CurrentBehaviour.BehaviourMask;
        public IActionBehaviour CurrentBehaviour { get; set; }
        public StopBehaviour StopBehaviour => stopBehaviour;
        public RunBehaviour RunBehaviour => runBehaviour;
        public StunBehaviour StunBehaviour => stunBehaviour;
        public KnockBackBehaviour KnockBackBehaviour => knockBackBehaviour;
        public DrawBehaviour DrawBehaviour => drawBehaviour;
        public DeadBehaviour DeadBehaviour => deadBehaviour;
        public SkillTable SkillTable => skillBehaviour;

        public void Rotate(Vector3 lookTarget) { Pathfinding.RotateToTarget(lookTarget); Animating.Flip(transform.forward); }
        public void Stop() => stopBehaviour.Stop();
        public void Run(Vector3 destination) => runBehaviour.Run(destination);
        public void Stun(float duration) => stunBehaviour.Stun(duration);
        public void KnockBack(Vector3 source, float distance, float duration) => knockBackBehaviour.KnockBack(source, distance, duration);
        public void Draw(Vector3 source, float duration) => DrawBehaviour.Draw(source, duration);
        public void Dead() => deadBehaviour.Dead();
        public void AddReward(System.Action action) => deadBehaviour.AddReward("Reward", action);
        
        
        /* Skill Behaviour */
        public void ActiveSkill(DataIndex actionCode, Vector3 targetPosition) => skillBehaviour.Active(actionCode, targetPosition);

        /*
         * Combat Status
         */
        public AliveValue Alive => combatStatus.Alive;
        public HpValue Hp => combatStatus.Hp;
        public ResourceValue Resource => combatStatus.Resource;
        public ShieldValue Shield => combatStatus.Shield;
        public StatTable StatTable => combatStatus.StatTable;
        public StatusEffectTable StatusEffectTable => combatStatus.StatusEffectTable;
        public Transform StatusEffectHierarchy => statusEffectHierarchy;
        public Transform Preposition(PrepositionType type) => prePosition.Get(type);
        
        public ActionTable<CombatEntity> OnCombatProvided { get; } = new();
        public ActionTable<CombatEntity> OnCombatTaken { get; } = new();

        public void TakeStatusEffect(StatusEffect effect) => StatusEffectTable[effect.DataIndex].Activate(this);
        public void DispelStatusEffect(DataIndex effectIndex) => StatusEffectTable[effectIndex]?.Dispel();


        public void Initialize()
        {
            StatTable.RegisterTable(Data.StaticStatTable);
            combatStatus.Initialize();
            skillBehaviour.Initialize(this);
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            combatStatus       = GetComponentInChildren<CharacterCombatStatus>();
            prePosition        = GetComponentInChildren<CharacterPreposition>();
            skillBehaviour     = GetComponentInChildren<SkillTable>();
            stopBehaviour      = GetComponentInChildren<StopBehaviour>();
            runBehaviour       = GetComponentInChildren<RunBehaviour>();
            stunBehaviour      = GetComponentInChildren<StunBehaviour>();
            knockBackBehaviour = GetComponentInChildren<KnockBackBehaviour>();
            drawBehaviour      = GetComponentInChildren<DrawBehaviour>();
            deadBehaviour      = GetComponentInChildren<DeadBehaviour>();
            searchEngine       = GetComponentInChildren<SearchEngine>();
            pathfinding        = GetComponentInChildren<PathfindingSystem>();
            animating          = GetComponentInChildren<AnimationModel>();
            
            Data.EditorSetUp();
        }
#endif
    }
}
