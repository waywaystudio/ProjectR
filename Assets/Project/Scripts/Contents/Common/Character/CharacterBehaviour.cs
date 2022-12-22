using System;
using System.Collections.Generic;
using Common.Character.Operation.Combat;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character
{
    public class CharacterBehaviour : MonoBehaviour, ICombatTaker, ICombatProvider
    {
        // TEST
        [SerializeField] private bool isAlive = true;
        [SerializeField] private float hp = 1000f;
        public StatTable StatTable { get; } = new();

        //
        private CombatValueEntity combatValue;
        private DefenseValueEntity defenseValue;

        public bool IsAlive { get => isAlive; set => isAlive = value; }
        
        public CombatValueEntity CombatValue
        {
            get
            {
                combatValue.Power = CombatPowerTable.Result * CombatPowerMultiTable.Result;
                combatValue.Critical = CriticalTable.Result * CriticalMultiTable.Result;
                combatValue.Haste = HasteTable.Result * HasteMultiTable.Result;
                combatValue.Hit = HitTable.Result * HitMultiTable.Result;
                // 치명타 데미지 증가
                // 특화....특화! 특화...

                return combatValue;
            }
            set => combatValue = value;
        }
        
        public DefenseValueEntity DefenseValue
        {
            get
            {
                defenseValue.Hp = hp;
                defenseValue.MaxHp = MaxHpTable.Result * MaxHpMultiTable.Result;
                defenseValue.MoveSpeed = MoveSpeedTable.Result * MoveSpeedMultiTable.Result;
                defenseValue.Armor = ArmorTable.Result * ArmorMultiTable.Result;
                defenseValue.Evade = EvadeTable.Result * EvadeMultiTable.Result;
                defenseValue.Resist = ResistTable.Result * ResistMultiTable.Result;
                // 받는 치명타 피해 관련
                // 치명타 받을 확률 관련

                return defenseValue;
            }
            set => defenseValue = value;
        }

        [SerializeField] private string characterName = string.Empty;
        [SerializeField] private int id;
        [SerializeField] private string combatClass;
        [SerializeField] private float searchingRange = 50f;
        [SerializeField] private LayerMask allyLayer;
        [SerializeField] private LayerMask enemyLayer;

        public string CharacterName => characterName ??= "Diablo";
        public int ID => id;

        public string ActionName => string.Empty;
        public string Name => CharacterName;
        public ICombatProvider Predecessor => this;
        
        public string CombatClass => combatClass ??= MainData.GetAdventurerData(CharacterName).CombatClass;
        public float SearchingRange => searchingRange;
        public LayerMask AllyLayer => allyLayer;
        public LayerMask EnemyLayer => enemyLayer;
        public virtual GameObject Object => gameObject;

        public FloatSumTable MaxHpTable { get; } = new();
        public FloatSumTable CombatPowerTable { get; } = new();
        public FloatSumTable MoveSpeedTable { get; } = new();
        public FloatSumTable CriticalTable { get; } = new();
        public FloatSumTable HasteTable { get; } = new();
        public FloatSumTable HitTable { get; } = new();
        public FloatSumTable EvadeTable { get; } = new();
        public FloatSumTable ArmorTable { get; } = new();
        public FloatSumTable ResistTable { get; } = new();
        public FloatMultiTable MaxHpMultiTable { get; } = new();
        public FloatMultiTable CombatPowerMultiTable { get; } = new();
        public FloatMultiTable MoveSpeedMultiTable { get; } = new();
        public FloatMultiTable CriticalMultiTable { get; } = new();
        public FloatMultiTable HasteMultiTable { get; } = new();
        public FloatMultiTable HitMultiTable { get; } = new();
        public FloatMultiTable EvadeMultiTable { get; } = new();
        public FloatMultiTable ArmorMultiTable { get; } = new();
        public FloatMultiTable ResistMultiTable { get; } = new();

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
        
        public List<GameObject> CharacterSearchedList { get; } = new();
        public List<GameObject> MonsterSearchedList { get; } = new();
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

        // public void TakeDamage(IDamageEntity damageEntity, IProvider provider)
        // {
            // Do Damaging
            // CombatUtility.TakeDamage(damageEntity, provider, this);
            //
            // var log = new CombatLog
            // {
            //     Provider = provider.Name,
            //     Taker = taker.Name,
            //     SkillName = damageEntity.ActionName,
            //     IsHit = false,
            //     IsCritical = false,
            //     Value = 0f,
            // };
            
            // IDamageEntity
            // ICombatEntity
            // ICombatEntity CommAttackEntity = new();
            // CommonAttackEntity.Clear();
            // CommonAttackEntity.Register(StatCode.MultiPower, "BaseAttribute", () => 1.4f, true);
            // CommonAttackEntity.Union(Cb);
        // }
        
        public virtual void TakeDamage(ICombatProvider provider)
        {
            // var log = new CombatLog
            // {
            //     Provider = provider.Name,
            //     Taker = CharacterName,
            //     SkillName = provider.ActionName,
            // };
            //
            // // Hit Chance
            // if (CombatUtility.IsHit(provider.CombatValue.Hit, Evade))
            // {
            //     log.IsHit = true;
            // }
            // else
            // {
            //     log.IsHit = false;
            //     provider.CombatReport(log);
            //     return;
            // }
            //
            // var damageAmount = provider.CombatValue.Power;
            //
            // // Critical
            // if (CombatUtility.IsCritical(provider.CombatValue.Critical))
            // {
            //     log.IsCritical = true;
            //     damageAmount *= 2f;
            // }
            //
            // // Armor
            // damageAmount = CombatUtility.ArmorReduce(Armor, damageAmount);
            //
            // Hp -= damageAmount;
            // log.Value = damageAmount;
            //
            // if (Hp <= 0.0d)
            // {
            //     Debug.Log("Dead!");
            //     log.Value -= Hp;
            //     IsAlive = false;
            // }
            //
            // provider.CombatReport(log);
        }

        public virtual void TakeSpell(ICombatProvider provider) {}
        public virtual void TakeHeal(ICombatProvider provider)
        {
            // var log = new CombatLog
            // {
            //     Provider = provider.Name,
            //     Taker = CharacterName,
            //     SkillName = provider.ActionName,
            // };
            //
            // var healAmount = provider.CombatValue.Power;
            //
            // if (CombatUtility.IsCritical(provider.CombatValue.Power))
            // {
            //     log.IsCritical = true;
            //     healAmount *= 2f;
            // }
            //
            // if (Hp + healAmount >= MaxHp)
            // {
            //     healAmount = MaxHp - Hp;
            // }
            //
            // Hp += healAmount;
            // log.Value = healAmount;
            //
            // provider.CombatReport(log);
        }
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
                      $"Provider:{log.Provider} Taker:{log.Taker} Skill:{log.SkillName}");
        }

#if UNITY_EDITOR
        [Button]
        private void Initialize() => Initialize(characterName);
#endif
    }
}
