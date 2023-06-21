using Common.Animation;
using Common.Skills;
using Common.Systems;
using UnityEngine;

namespace Common.Characters
{
    using Behaviours;

    public class CharacterBehaviour : MonoBehaviour, ICombatExecutor, ICharacterSystem, IEditable
    {
        [SerializeField] protected CharacterAnimation animationTable;
        [SerializeField] protected CharacterCombatStatus combatStatus;
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

        /*
         * Common Attribute
         */
        public virtual DataIndex DataIndex => DataIndex.None;
        public virtual CharacterMask CombatClass => CharacterMask.Mage;
        public virtual string Name => "characterName";
        public CharacterData Data { get; set; }
        public Vector3 Position => transform.position;
        public SearchingSystem Searching => searching;
        public CollidingSystem Colliding => colliding;
        public PathfindingSystem Pathfinding => pathfinding;
        // public AnimationModel Animating => animating;
        public virtual IAnimator Animating => animating;
        public Transform DamageSpawn => damageSpawn;

        /*
         * Behaviour Attribute
         */
        [Sirenix.OdinInspector.ShowInInspector]
        public ActionMask BehaviourMask => CurrentBehaviour is null ? ActionMask.None : CurrentBehaviour.BehaviourMask;
        public IActionBehaviour CurrentBehaviour { get; set; }
        public CharacterAnimation AnimationTable => animationTable;

        public virtual void Rotate(Vector3 lookTarget)
        {
            Pathfinding.RotateToTarget(lookTarget);
        }

        public void Stop()
        {
            stopBehaviour.Stop();
        }

        public void Run(Vector3 destination)
        {
            runBehaviour.Run(destination);
        }

        public void Stun(float duration)
        {
            stunBehaviour.Stun(duration);
        }

        public void KnockBack(Vector3 source, float distance, float duration)
        {
            knockBackBehaviour.KnockBack(source, distance, duration);
        }

        public void Dead()
        {
            deadBehaviour.Dead();
        }
        
        public void AddReward(System.Action action) => deadBehaviour.AddReward("Reward", action);
        
        
        /* Skill Behaviour */
        public SkillComponent GetSkill(DataIndex actionCode) => skillBehaviour.GetSkill(actionCode);
        public SkillBehaviour SkillBehaviour => skillBehaviour;
        public void ExecuteSkill(DataIndex actionCode, Vector3 targetPosition) => skillBehaviour.Active(actionCode, targetPosition);
        public void CancelSkill() => skillBehaviour.Cancel();
        public void ReleaseSkill() => skillBehaviour.Release();

        /*
         * Combat Status
         */
        public IDynamicStatEntry DynamicStatEntry => combatStatus ??= GetComponentInChildren<CharacterCombatStatus>();
        public StatTable StatTable => DynamicStatEntry.StatTable;
        public Transform StatusEffectHierarchy => statusEffectHierarchy;

        // public CombatMultiplier DamageMultiplier { get; } = new();
        // public CombatMultiplier HealMultiplier;
        public ActionTable<CombatEntity> OnDamageProvided { get; } = new();
        public ActionTable<CombatEntity> OnDamageTaken { get; } = new();
        public ActionTable<CombatEntity> OnHealProvided { get; } = new();
        public ActionTable<CombatEntity> OnHealTaken { get; } = new();
        public ActionTable<IStatusEffect> OnDeBuffProvided { get; } = new();
        public ActionTable<IStatusEffect> OnDeBuffTaken { get; } = new();
        public ActionTable<IStatusEffect> OnBuffProvided { get; } = new();
        public ActionTable<IStatusEffect> OnBuffTaken { get; } = new();

        
        private void OnEnable()
        {
            OnDeBuffTaken.Add("RegisterTable", combatStatus.DeBuffTable.Register);
            OnBuffTaken.Add("RegisterTable", combatStatus.BuffTable.Register);
        }

        private void OnDisable()
        {
            OnDeBuffTaken.Remove("RegisterTable");
            OnBuffTaken.Remove("RegisterTable");
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            combatStatus       ??= GetComponentInChildren<CharacterCombatStatus>();
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
