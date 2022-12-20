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
        public double Hp { get => hp; set => hp = value; }
        //
        
        [SerializeField] private string characterName = string.Empty;
        [SerializeField] private int id;
        [SerializeField] private string combatClass;
        [SerializeField] private float searchingRange = 50f;
        [SerializeField] private LayerMask allyLayer;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private DoubleTable maxHp = new();
        [SerializeField] private FloatTable combatPowerTable = new();
        [SerializeField] private FloatTable moveSpeedTable = new();
        [SerializeField] private FloatTable criticalTable = new();
        [SerializeField] private FloatTable hasteTable = new();
        [SerializeField] private FloatTable hitTable = new();
        [SerializeField] private FloatTable evadeTable = new();
        [SerializeField] private FloatTable armorTable = new();

        public string CharacterName => characterName ??= "Diablo";
        public int ID => id;

        public string CombatClass => combatClass ??= MainData.GetAdventurerData(CharacterName).CombatClass;
        public float SearchingRange => searchingRange;
        public LayerMask AllyLayer => allyLayer;
        public LayerMask EnemyLayer => enemyLayer;

        public ActionTable OnStart { get; } = new();
        public ActionTable OnUpdate { get; } = new();
        public ActionTable OnIdle { get; } = new();
        public ActionTable<Vector3, Action> OnWalk { get; } = new();
        public ActionTable<Vector3, Action> OnRun { get; } = new();
        public ActionTable<Vector3> OnTeleport { get; } = new();
        public ActionTable<string, Action> OnSkill { get; } = new();
        public ActionTable OnSkillHit { get; } = new(1);
        public ActionTable<string, ICombatProvider> OnTakeBuff { get; } = new();
        public ActionTable<string, ICombatProvider> OnTakeDeBuff { get; } = new();
        public ActionTable<CombatLog> OnReportDamage { get; } = new();
        public ActionTable<CombatLog> OnReportHeal { get; } = new();
        public ActionTable<CombatLog> OnReportStatusEffect { get; } = new();

        public FunctionTable<bool> IsReached { get; } = new();
        public FunctionTable<Vector3> Direction { get; } = new();
        public DoubleTable MaxHp => maxHp;
        public FloatTable CombatPowerTable => combatPowerTable;
        public FloatTable MoveSpeedTable => moveSpeedTable;
        public FloatTable CriticalTable => criticalTable;
        public FloatTable HasteTable => hasteTable;
        public FloatTable HitTable => hitTable;
        public FloatTable EvadeTable => evadeTable;
        public FloatTable ArmorTable => armorTable;

        public List<GameObject> CharacterSearchedList { get; } = new();
        public List<GameObject> MonsterSearchedList { get; } = new();
        public ICombatTaker MainTarget { get; set; }

        public void Idle() => OnIdle?.Invoke();
        public void Walk(Vector3 destination, Action pathfindingCallback = null) => OnWalk?.Invoke(destination, pathfindingCallback);
        public void Run(Vector3 destination, Action pathfindingCallback = null) => OnRun?.Invoke(destination, pathfindingCallback);
        public void Teleport(Vector3 destination) => OnTeleport?.Invoke(destination);
        public void Skill(string skillName, Action animationCallback) => OnSkill?.Invoke(skillName, animationCallback);
        public void SkillHit() => OnSkillHit?.Invoke();

        public void Initialize(string character)
        {
            var profile = MainData.GetAdventurerData(character);

            characterName = profile.Name;
            id = profile.ID;
            combatClass = profile.CombatClass;
        }

        protected virtual void Start()
        {
            Initialize(CharacterName);
            
            OnReportDamage.Register(GetInstanceID(), ShowLog);
            OnStart?.Invoke();
        }

        protected void Update() => OnUpdate?.Invoke();

        public virtual GameObject Taker => gameObject;
        public void ReportDamage(CombatLog log) => OnReportDamage?.Invoke(log);
        public void ReportHeal(CombatLog log) => OnReportHeal?.Invoke(log);
        public void ReportStatusEffect(CombatLog log) => OnReportStatusEffect?.Invoke(log);
        public virtual void TakeDamage(ICombatProvider combatInfo)
        {
            var log = new CombatLog
            {
                Provider = combatInfo.ProviderName,
                Taker = CharacterName,
                SkillName = combatInfo.Name,
            };

            
            // Hit Chance
            var hitChance = UnityEngine.Random.Range(0f, 1.0f);
            if (hitChance > combatInfo.Hit)
            {
                log.IsHit = true;
                combatInfo.CombatReport(log);
                return;
            }

            log.IsHit = true;
            float damageAmount;

            // Critical
            if (UnityEngine.Random.Range(0f, 1.0f) > combatInfo.Critical)
            {
                log.IsCritical = true;
                damageAmount = combatInfo.CombatPower * 2f;
            }
            else
            {
                damageAmount = combatInfo.CombatPower;
            }
            
            // Armor
            damageAmount = CombatManager.ArmorReduce(ArmorTable.Result, damageAmount);
            
            Hp -= damageAmount;
            log.Value = damageAmount;

            if (Hp <= 0.0d)
            {
                Debug.Log("Dead!");
                log.Value -= (float)Hp;
                IsAlive = false;
            }
            
            combatInfo.CombatReport(log);
        }
        
        public virtual void TakeHeal(ICombatProvider healInfo)
        {
            float healValue;
            
            if (UnityEngine.Random.Range(0f, 1.0f) > healInfo.Critical)
            {
                healValue = healInfo.CombatPower * 2f; 
            }
            else
            {
                healValue = healInfo.CombatPower;
            }

            Hp += healValue;
        }
        public virtual void TakeBuff(string key, ICombatProvider statusEffect) => OnTakeBuff?.Invoke(key, statusEffect);
        public virtual void TakeDeBuff(string key, ICombatProvider statusEffect) => OnTakeDeBuff?.Invoke(key, statusEffect);
        
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
