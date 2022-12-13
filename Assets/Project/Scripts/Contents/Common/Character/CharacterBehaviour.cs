using System;
using System.Collections.Generic;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character
{
    public class CharacterBehaviour : MonoBehaviour, ICombatTaker
    {
        [SerializeField] private string characterName = string.Empty;
        [SerializeField] private int id;
        [SerializeField] private string combatClass;
        [SerializeField] private string role;
        [SerializeField] private float baseMoveSpeed = 7;
        [SerializeField] private float baseAttackSpeed = 1f;
        [SerializeField] private float searchingRange;
        [SerializeField] private LayerMask allyLayer;
        [SerializeField] private LayerMask enemyLayer;

        // TEST
        [ShowInInspector] private Dictionary<Type, Action> typeActionTable = new(4);
        [SerializeField] private bool isAlive = true;
        [SerializeField] private double hp = 50;

        public Dictionary<Type, Action> TypeActionTable => typeActionTable;
        //
        
        public string CharacterName => characterName;
        public int ID => id;
        public string CombatClass => combatClass;
        public string Role => role;
        public float SearchingRange => searchingRange;
        public LayerMask AllyLayer => allyLayer;
        public LayerMask EnemyLayer => enemyLayer;

        public ActionTable OnUpdate { get; } = new();
        public ActionTable OnIdle { get; } = new();
        public ActionTable<Vector3> OnWalk { get; } = new();
        public ActionTable<Vector3> OnRun { get; } = new();
        public ActionTable<string, Action> OnAttack { get; } = new();
        public ActionTable<string, Action> OnSkill { get; } = new();
        public ActionTable<string, Action> OnChanneling { get; } = new();
        public ActionTable OnAttackHit { get; } = new(1);
        public ActionTable OnSkillHit { get; } = new(1);
        public ActionTable OnChannelingHit { get; } = new(1);
        public ActionTable OnLookLeft { get; } = new();
        public ActionTable OnLookRight { get; } = new();

        public FunctionTable<bool> IsReached { get; } = new();
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public double Hp { get => Math.Max(0d, hp); set => hp = Math.Max(0d, value); }
        public float MoveSpeed { get => baseMoveSpeed; set => baseMoveSpeed = value; }
        public float AttackSpeed { get => baseAttackSpeed; set => baseAttackSpeed = value; } // 이게 무슨 의미가 있을라나...가속 계수가 되려나

        public List<GameObject> CharacterSearchedList { get; } = new();
        public List<GameObject> MonsterSearchedList { get; } = new();

        public void Idle() => OnIdle?.Invoke();
        public void Walk(Vector3 destination) => OnWalk?.Invoke(destination);
        public void Run(Vector3 destination) => OnRun?.Invoke(destination);
        public void Attack(string skillName, Action callback) => OnAttack?.Invoke(skillName, callback);
        public void Skill(string skillName, Action callback) => OnSkill?.Invoke(skillName, callback);
        public void Channeling(string skillName, Action callback) => OnChanneling?.Invoke(skillName, callback);
        
        public void AttackHit() => OnAttackHit?.Invoke();
        public void SkillHit() => OnSkillHit?.Invoke();
        public void ChannelingHit() => OnChannelingHit?.Invoke();
        public void LookLeft() => OnLookLeft?.Invoke();
        public void LookRight() => OnLookRight?.Invoke();

        public void Initialize(string character)
        {
            var profile = MainData.GetAdventurerData(character);

            characterName = profile.Name;
            id = profile.ID;
            combatClass = profile.Job;
            role = profile.Role;

            var classData = MainData.GetCombatClassData(combatClass);

            baseAttackSpeed = classData.AttackSpeed;
        }


        private void Awake()
        {
            // TODO. OnTest
            Initialize(characterName ?? "Diablo");
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        public virtual GameObject Taker => gameObject;
        public virtual void TakeDamage(IDamageProvider damageInfo) {}
        public virtual void TakeHeal(IHealProvider healInfo)
        {
            double healValue;
            
            if (UnityEngine.Random.Range(0f, 1.0f) > healInfo.Critical)
            {
                healValue = healInfo.CombatValue * 2d; 
            }
            else
            {
                healValue = healInfo.CombatValue;
            }

            Hp += healValue;
            
            // Debug.Log($"{CharacterName} healed {healValue} by {healInfo.Provider.GetComponent<CharacterBehaviour>().CharacterName}");
        }
        public virtual void TakeExtra(IExtraProvider extra) {}
    }
}
