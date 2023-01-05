using System;
using System.Collections.Generic;
using Character.Combat;
using Core;
using UnityEngine;

// ReSharper disable UnusedMember.Local

namespace Character
{
    public class CharacterBehaviour : MonoBehaviour, ICombatTaker, ICombatProvider, ISearchedListTaker, IEditorSetUp
    {
        [SerializeField] protected string characterName = string.Empty;
        [SerializeField] protected IDCode id;
        [SerializeField] protected Status status = new ();

        public IDCode ID => id;
        public Status Status => status;
        public StatTable StatTable { get; } = new();
        public string Name => characterName ??= "Diablo";
        public IDCode ActionCode => IDCode.None;
        public ICombatProvider Provider => this;
        public GameObject Object => gameObject;

        public ActionTable OnStart { get; } = new();
        public ActionTable OnUpdate { get; } = new();
        
        public ActionTable OnIdle { get; } = new();
        public ActionTable<Vector3, Action> OnWalk { get; } = new();
        public ActionTable<Vector3, Action> OnRun { get; } = new();
        public ActionTable<Vector3> OnTeleport { get; } = new();
        public ActionTable<string, Action> OnSkill { get; } = new(8);
        public ActionTable OnSkillHit { get; } = new(4);
        
        public ActionTable<ICombatEntity> OnTakeDamage { get; } = new();
        public ActionTable<ICombatEntity> OnTakeStatusEffect { get; } = new();
        public ActionTable<IDCode> OnDispelStatusEffect { get; } = new();
        public ActionTable<CombatLog> OnCombatActive { get; } = new();
        public ActionTable<CombatLog> OnCombatPassive { get; } = new();
        
        public FunctionTable<bool> IsReached { get; } = new();
        public FunctionTable<Vector3> Direction { get; } = new();

        public CombatBehaviour CombatBehaviour { get; set; }
        
        public List<ICombatTaker> AdventurerList { get; set; }
        public List<ICombatTaker> MonsterList { get; set; }
        public ICombatTaker MainTarget { get; set; }
        public ICombatTaker Self => this;

        public void Idle() => OnIdle?.Invoke();
        public void Walk(Vector3 destination, Action pathCallback = null) => OnWalk?.Invoke(destination, pathCallback);
        public void Run(Vector3 destination, Action pathCallback = null) => OnRun?.Invoke(destination, pathCallback);
        public void Teleport(Vector3 destination) => OnTeleport?.Invoke(destination);
        public void Skill(string skillName, Action animationCallback) => OnSkill?.Invoke(skillName, animationCallback);
        public void SkillHit() => OnSkillHit?.Invoke();

        public void TakeDamage(ICombatEntity provider) => OnTakeDamage.Invoke(provider);
        public void TakeSpell(ICombatEntity provider) => CombatUtility.TakeSpell(provider, this);
        public void TakeHeal(ICombatEntity provider) => CombatUtility.TakeHeal(provider, this);
        public void TakeStatusEffect(ICombatEntity statusEffect) => OnTakeStatusEffect?.Invoke(statusEffect);
        public void DispelStatusEffect(IDCode code) => OnDispelStatusEffect?.Invoke(code);

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
        public virtual void SetUp()
        {
            if (characterName == string.Empty)
            {
                Debug.LogError("CharacterName Required");
            }
        }
#endif
    }
}
