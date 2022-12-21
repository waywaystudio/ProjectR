using System;
using System.Collections.Generic;
using Core;
using MainGame;
using MainGame.Manager.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character
{
    public class CharacterBehaviour : MonoBehaviour, ICombatTaker
    {
        // TEST
        [SerializeField] private bool isAlive = true;
        [SerializeField] private double hp = 1000;
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public double Hp { get => hp; set => hp = Math.Min(MaxHp, value); }
        //
        
        [SerializeField] private string characterName = string.Empty;
        [SerializeField] private int id;
        [SerializeField] private string combatClass;
        [SerializeField] private float searchingRange = 50f;
        [SerializeField] private LayerMask allyLayer;
        [SerializeField] private LayerMask enemyLayer;

        public string CharacterName => characterName ??= "Diablo";
        public int ID => id;
        public string CombatClass => combatClass ??= MainData.GetAdventurerData(CharacterName).CombatClass;
        public float SearchingRange => searchingRange;
        public LayerMask AllyLayer => allyLayer;
        public LayerMask EnemyLayer => enemyLayer;
        public virtual GameObject Object => gameObject;
        
        public DoubleSumTable MaxHpTable { get; } = new();
        public FloatSumTable CombatPowerTable { get; } = new();
        public FloatSumTable MoveSpeedTable { get; } = new();
        public FloatSumTable CriticalTable { get; } = new();
        public FloatSumTable HasteTable { get; } = new();
        public FloatSumTable HitTable { get; } = new();
        public FloatSumTable EvadeTable { get; } = new();
        public FloatSumTable ArmorTable { get; } = new();
        public DoubleMultiTable MaxHpMultiTable { get; } = new();
        public FloatMultiTable CombatPowerMultiTable { get; } = new();
        public FloatMultiTable MoveSpeedMultiTable { get; } = new();
        public FloatMultiTable CriticalMultiTable { get; } = new();
        public FloatMultiTable HasteMultiTable { get; } = new();
        public FloatMultiTable HitMultiTable { get; } = new();
        public FloatMultiTable EvadeMultiTable { get; } = new();
        public FloatMultiTable ArmorMultiTable { get; } = new();

        public double MaxHp => MaxHpTable.Result * MaxHpMultiTable.Result;
        public float CombatPower => CombatPowerTable.Result * CombatPowerMultiTable.Result;
        public float MoveSpeed => MoveSpeedTable.Result * MoveSpeedMultiTable.Result;
        public float Critical => CriticalTable.Result * CriticalMultiTable.Result;
        public float Haste => HasteTable.Result * HasteMultiTable.Result;
        public float Hit => HitTable.Result * HitMultiTable.Result;
        public float Evade => EvadeTable.Result * EvadeMultiTable.Result;
        public float Armor => ArmorTable.Result * ArmorMultiTable.Result;

        public ActionTable OnStart { get; } = new();
        public ActionTable OnUpdate { get; } = new();
        public ActionTable OnIdle { get; } = new();
        public ActionTable<Vector3, Action> OnWalk { get; } = new();
        public ActionTable<Vector3, Action> OnRun { get; } = new();
        public ActionTable<Vector3> OnTeleport { get; } = new();
        public ActionTable<string, Action> OnSkill { get; } = new();
        public ActionTable OnSkillHit { get; } = new(1);
        public ActionTable<ICombatProvider> OnTakeBuff { get; } = new();
        public ActionTable<ICombatProvider> OnTakeDeBuff { get; } = new();
        public ActionTable<CombatLog> OnCombatReporting { get; } = new();
        public FunctionTable<bool> IsReached { get; } = new();
        public FunctionTable<Vector3> Direction { get; } = new();
        
        public List<GameObject> CharacterSearchedList { get; } = new();
        public List<GameObject> MonsterSearchedList { get; } = new();
        public ICombatTaker MainTarget { get; set; }

        public void Idle() => OnIdle?.Invoke();
        public void Walk(Vector3 destination, Action pathfindingCallback = null) => OnWalk?.Invoke(destination, pathfindingCallback);
        public void Run(Vector3 destination, Action pathfindingCallback = null) => OnRun?.Invoke(destination, pathfindingCallback);
        public void Teleport(Vector3 destination) => OnTeleport?.Invoke(destination);
        public void Skill(string skillName, Action animationCallback) => OnSkill?.Invoke(skillName, animationCallback);
        public void SkillHit() => OnSkillHit?.Invoke();
        public void CombatReport(ILog log) => OnCombatReporting?.Invoke(log as CombatLog);

        public void Initialize(string character)
        {
            var profile = MainData.GetAdventurerData(character);

            characterName = profile.Name;
            id = profile.ID;
            combatClass = profile.CombatClass;
        }
        
        public virtual void TakeDamage(ICombatProvider provider)
        {
            var log = new CombatLog
            {
                Provider = provider.ProviderName,
                Taker = CharacterName,
                SkillName = provider.ActionName,
            };

            // Hit Chance
            if (CombatManager.IsHit(provider.Hit, Evade))
            {
                log.IsHit = true;
            }
            else
            {
                log.IsHit = false;
                provider.CombatReport(log);
                return;
            }

            var damageAmount = provider.CombatPower;

            // Critical
            if (CombatManager.IsCritical(provider.CombatPower))
            {
                log.IsCritical = true;
                damageAmount *= 2f;
            }

            // Armor
            damageAmount = CombatManager.ArmorReduce(Armor, damageAmount);
            
            Hp -= damageAmount;
            log.Value = damageAmount;

            if (Hp <= 0.0d)
            {
                Debug.Log("Dead!");
                log.Value -= (float)Hp;
                IsAlive = false;
            }
            
            provider.CombatReport(log);
        }
        
        public virtual void TakeHeal(ICombatProvider provider)
        {
            var log = new CombatLog
            {
                Provider = provider.ProviderName,
                Taker = CharacterName,
                SkillName = provider.ActionName,
            };

            var healAmount = provider.CombatPower;

            if (CombatManager.IsCritical(provider.CombatPower))
            {
                log.IsCritical = true;
                healAmount *= 2f;
            }

            if (Hp + healAmount >= MaxHp)
            {
                healAmount = (float)(MaxHp - Hp);
            }

            Hp += healAmount;
            log.Value = healAmount;

            provider.CombatReport(log);
        }
        public virtual void TakeBuff(ICombatProvider statusEffect) => OnTakeBuff?.Invoke(statusEffect);
        public virtual void TakeDeBuff(ICombatProvider statusEffect) => OnTakeDeBuff?.Invoke(statusEffect);
        
        
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
                      $"Provider:{log.Provider} Taker:{log.Taker} Skill:{log.SkillName}");
        }

#if UNITY_EDITOR
        [Button]
        private void Initialize() => Initialize(characterName);
#endif
    }
}
