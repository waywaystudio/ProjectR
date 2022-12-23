using System;
using System.Collections.Generic;
using Common.Character.Operation.Combat;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character
{
    public class CharacterBehaviour : MonoBehaviour, ICombatTaker, ICombatProvider, ISearchable
    {
        [SerializeField] private string characterName = string.Empty;
        [SerializeField] private int id;
        [SerializeField] private string combatClass;
        [SerializeField] private float searchingRange = 50f;
        [SerializeField] private LayerMask allyLayer;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private bool isAlive = true;
        [SerializeField] private float hp = 1000f;
        
        public string CharacterName => characterName ??= "Diablo";
        public int ID => id;
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public float Hp { get => hp; set => hp = Mathf.Min(StatTable.MaxHp, value); }
        [ShowInInspector]
        public StatTable StatTable { get; } = new();
        public string ActionName => string.Empty;
        public string Name => CharacterName;
        public ICombatProvider Sender => this;
        
        public string CombatClass => combatClass ??= MainData.GetAdventurerData(CharacterName).CombatClass;
        public float SearchingRange => searchingRange;
        public LayerMask AllyLayer => allyLayer;
        public LayerMask EnemyLayer => enemyLayer;
        public virtual GameObject Object => gameObject;

        public ActionTable OnStart { get; } = new();
        public ActionTable OnUpdate { get; } = new();
        
        public ActionTable OnIdle { get; } = new();
        public ActionTable<Vector3, Action> OnWalk { get; } = new();
        public ActionTable<Vector3, Action> OnRun { get; } = new();
        public ActionTable<Vector3> OnTeleport { get; } = new();
        public ActionTable<string, Action> OnSkill { get; } = new();
        public ActionTable OnSkillHit { get; } = new(1);
        public ActionTable<ICombatProvider> OnTakeStatusEffect { get; } = new();
        public ActionTable<CombatLog> OnCombatReporting { get; } = new();
        public FunctionTable<bool> IsReached { get; } = new();
        public FunctionTable<Vector3> Direction { get; } = new();
        
        public List<GameObject> AdventureList { get; } = new();
        public List<GameObject> MonsterList { get; } = new();
        public ICombatTaker MainTarget { get; set; }

        public void Idle() => OnIdle?.Invoke();
        public void Walk(Vector3 destination, Action pathCallback = null) => OnWalk?.Invoke(destination, pathCallback);
        public void Run(Vector3 destination, Action pathCallback = null) => OnRun?.Invoke(destination, pathCallback);
        public void Teleport(Vector3 destination) => OnTeleport?.Invoke(destination);
        public void Skill(string skillName, Action animationCallback) => OnSkill?.Invoke(skillName, animationCallback);
        public void SkillHit() => OnSkillHit?.Invoke();
        public void CombatReport(CombatLog log) => OnCombatReporting?.Invoke(log);

        public void Initialize(string character)
        {
            var profile = MainData.GetAdventurerData(character);

            characterName = profile.Name;
            id = profile.ID;
            combatClass = profile.CombatClass;
        }

        public virtual void TakeDamage(ICombatProvider provider) => CombatUtility.TakeDamage(provider, this);
        public virtual void TakeSpell(ICombatProvider provider) => CombatUtility.TakeSpell(provider, this);
        public virtual void TakeHeal(ICombatProvider provider) => CombatUtility.TakeHeal(provider, this);
        public virtual void TakeStatusEffect(ICombatProvider statusEffect) => OnTakeStatusEffect?.Invoke(statusEffect);

        protected virtual void Start()
        {
            Initialize(CharacterName);
            
            OnCombatReporting.Register(GetInstanceID(), ShowLog);
            OnStart?.Invoke();
        }

        protected void Update() => OnUpdate?.Invoke();
        
        private void ShowLog(CombatLog log)
        {
            Debug.Log($"Combat : IsHit:{log.IsHit} IsCritical:{log.IsCritical} Value:{log.Value} " +
                      $"Provider:{log.Provider} Taker:{log.Taker} Skill:{log.ActionName}");
        }

#if UNITY_EDITOR
        [Button]
        private void Initialize() => Initialize(characterName);
#endif
    }
}
