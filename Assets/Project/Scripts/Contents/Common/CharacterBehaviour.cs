using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

// ReSharper disable UnusedMember.Local

namespace Common
{
    using Character;
    using Character.Operation;
    using Character.Operation.Combat;
    
    public class CharacterBehaviour : MonoBehaviour, ICombatTaker, ICombatProvider, ISearchable
    {
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected IDCode id;
        [SerializeField] protected float searchingRange = 50f;
        [SerializeField] protected LayerMask allyLayer;
        [SerializeField] protected LayerMask enemyLayer;
        [SerializeField] protected Status status = new ();

        public IDCode ID => id;
        public Status Status => status;
        public StatTable StatTable { get; } = new();
        public string Name => characterName ??= "Diablo";
        public IDCode ActionCode => IDCode.None;
        public ICombatProvider Sender => this;
        public float SearchingRange => searchingRange;
        public LayerMask AllyLayer => allyLayer;
        public LayerMask EnemyLayer => enemyLayer;
        public GameObject Object => gameObject;

        public ActionTable OnStart { get; } = new();
        public ActionTable OnUpdate { get; } = new();
        public ActionTable OnIdle { get; } = new();
        public ActionTable<Vector3, Action> OnWalk { get; } = new();
        public ActionTable<Vector3, Action> OnRun { get; } = new();
        public ActionTable<Vector3> OnTeleport { get; } = new();
        public ActionTable<string, Action> OnSkill { get; } = new();
        public ActionTable OnSkillHit { get; } = new();
        public ActionTable<ICombatProvider> OnTakeDamage { get; } = new();
        public ActionTable<ICombatProvider> OnTakeStatusEffect { get; } = new();
        public ActionTable<CombatLog> OnCombatActive { get; } = new();
        public ActionTable<CombatLog> OnCombatPassive { get; } = new();
        public FunctionTable<bool> IsReached { get; } = new();
        public FunctionTable<Vector3> Direction { get; } = new();

        public CombatOperation CombatOperation { get; set; }
        
        public List<GameObject> AdventurerList { get; } = new();
        public List<GameObject> MonsterList { get; } = new();
        public ICombatTaker MainTarget { get; set; }
        public ICombatTaker Self => this;

        public void Idle() => OnIdle?.Invoke();
        public void Walk(Vector3 destination, Action pathCallback = null) => OnWalk?.Invoke(destination, pathCallback);
        public void Run(Vector3 destination, Action pathCallback = null) => OnRun?.Invoke(destination, pathCallback);
        public void Teleport(Vector3 destination) => OnTeleport?.Invoke(destination);
        public void Skill(string skillName, Action animationCallback) => OnSkill?.Invoke(skillName, animationCallback);
        public void SkillHit() => OnSkillHit?.Invoke();
        public void ReportActive(CombatLog log) => OnCombatActive.Invoke(log);
        public void ReportPassive(CombatLog log) => OnCombatPassive.Invoke(log);

        public virtual void TakeDamage(ICombatProvider provider) => OnTakeDamage.Invoke(provider);
        public virtual void TakeSpell(ICombatProvider provider) => CombatUtility.TakeSpell(provider, this);
        public virtual void TakeHeal(ICombatProvider provider) => CombatUtility.TakeHeal(provider, this);
        public virtual void TakeStatusEffect(ICombatProvider statusEffect) => OnTakeStatusEffect?.Invoke(statusEffect);

        protected virtual void Start()
        {
            // OnCombatActive.Register(GetInstanceID(), ShowLog);
            OnTakeDamage.Register(GetInstanceID(), (provider) => CombatUtility.TakeDamage(provider, this));
            OnStart?.Invoke();

            status.StatTable = StatTable;
            status.Hp = StatTable.MaxHp;
            status.IsAlive = true;
        }
        protected virtual void Update() => OnUpdate?.Invoke();
        
        private void ShowLog(CombatLog log)
        {
            Debug.Log($"Combat : IsHit:{log.IsHit} IsCritical:{log.IsCritical} Value:{log.Value} " +
                      $"Provider:{log.Provider} Taker:{log.Taker} Skill:{log.ActionName}");
        }

#if UNITY_EDITOR
        protected virtual void SetUp()
        {
            if (characterName != string.Empty) return;
            
            Debug.LogError("CharacterName Required");
        }
#endif
    }
}
