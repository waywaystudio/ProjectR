using System;
using System.Collections.Generic;
using Common.Character.Operation;
using Core;
using MainGame;
using UnityEngine;
// ReSharper disable UnusedMember.Local

namespace Common.Character
{
    using Operation.Combat;
    
    public class CharacterBehaviour : MonoBehaviour, ICombatTaker, ICombatProvider, ISearchable
    {
        [SerializeField] private string characterName = string.Empty;
        [SerializeField] private IDCode id;
        [SerializeField] private IDCode combatClassID;
        [SerializeField] private string role;
        [SerializeField] private float searchingRange = 50f;
        [SerializeField] private LayerMask allyLayer;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private Status status = new ();

        public IDCode ID => id;
        public string Role => role;
        public Status Status => status;
        public StatTable StatTable { get; } = new();
        public IDCode ActionCode => IDCode.None;
        public string Name => characterName ??= "Diablo";
        public ICombatProvider Sender => this;
        public IDCode CombatClassID => combatClassID;
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
        protected void Update() => OnUpdate?.Invoke();
        private void ShowLog(CombatLog log)
        {
            Debug.Log($"Combat : IsHit:{log.IsHit} IsCritical:{log.IsCritical} Value:{log.Value} " +
                      $"Provider:{log.Provider} Taker:{log.Taker} Skill:{log.ActionName}");
        }

#if UNITY_EDITOR

        public ActionTable EditorInitialize { get; } = new();
        private void Initialize()
        {
            if (characterName == string.Empty)
            {
                Debug.LogError("CharacterName Required");
                return;
            }
            var profile = MainData.GetAdventurer(characterName.ToEnum<IDCode>());

            id = (IDCode)profile.ID;
            role = profile.Role;
            combatClassID = (IDCode)profile.CombatClassId;
            EditorInitialize?.Invoke();
        }
#endif
    }
}
