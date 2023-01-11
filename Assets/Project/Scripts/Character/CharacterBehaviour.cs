using System;
using Core;
using UnityEngine;

namespace Character
{
    public class CharacterBehaviour : MonoBehaviour, ICombatExecutor, IEditorSetUp
    {
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected DataIndex dataIndex;

        public string Name => characterName ??= "Diablo";
        public DataIndex DataIndex => dataIndex;
        public DataIndex ActionCode => DataIndex.None;
        public IDynamicStatEntry DynamicStatEntry { get; set; }
        public ICombatProvider Provider => this;
        public GameObject Object => gameObject;
        public StatTable StatTable { get; } = new(1);

        public ActionTable OnUpdate { get; } = new();
        public ActionTable OnIdle { get; } = new();
        public ActionTable<Vector3, Action> OnWalk { get; } = new();
        public ActionTable<Vector3, Action> OnRun { get; } = new();
        public ActionTable<Vector3> OnTeleport { get; } = new();
        public ActionTable<string, Action> OnSkill { get; } = new(8);
        public ActionTable OnSkillHit { get; } = new(4);
        public ActionTable<IStatusEffect> OnTakeStatusEffect { get; } = new(2);
        public ActionTable<DataIndex> OnDispelStatusEffect { get; } = new(2);
        public ActionTable<CombatLog> OnCombatActive { get; } = new(4);
        public ActionTable<CombatLog> OnCombatPassive { get; } = new(4);

        public ICombatBehaviour CombatBehaviour { get; set; }
        public ISkill SkillInfo { get; set; }
        public ISearching SearchingEngine { get; set; }
        public ITargeting TargetingEngine { get; set; }
        public IPathfinding PathfindingEngine { get; set; }

        public void Idle() => OnIdle?.Invoke();
        public void Walk(Vector3 destination, Action pathCallback = null) => OnWalk?.Invoke(destination, pathCallback);
        public void Run(Vector3 destination, Action pathCallback = null) => OnRun?.Invoke(destination, pathCallback);
        public void Teleport(Vector3 destination) => OnTeleport?.Invoke(destination);
        public void Skill(string skillName, Action animationCallback) => OnSkill?.Invoke(skillName, animationCallback);
        public void SkillHit() => OnSkillHit?.Invoke();

        public void TakeDamage(ICombatTable provider) => CombatUtility.TakeDamage(provider, this);
        public void TakeSpell(ICombatTable provider) => CombatUtility.TakeSpell(provider, this);
        public void TakeHeal(ICombatTable provider) => CombatUtility.TakeHeal(provider, this);
        public void TakeStatusEffect(IStatusEffect statusEffect) => OnTakeStatusEffect?.Invoke(statusEffect);
        public void DispelStatusEffect(DataIndex code) => OnDispelStatusEffect?.Invoke(code);

        protected virtual void Update() { OnUpdate?.Invoke(); }
        
#if UNITY_EDITOR
        public virtual void SetUp()
        {
            if (characterName == string.Empty) Debug.LogError("CharacterName Required");
        }
        // private void ShowLog(CombatLog log)
        // {
        //     Debug.Log($"Combat : IsHit:{log.IsHit} IsCritical:{log.IsCritical} Value:{log.Value} " +
        //               $"Provider:{log.Provider} Taker:{log.Taker} Skill:{log.ActionName}");
        // }
#endif
    }
}
