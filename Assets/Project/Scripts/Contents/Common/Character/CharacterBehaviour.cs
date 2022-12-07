using System;
using System.Collections.Generic;
using Core;
using MainGame;
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
        [SerializeField] private float baseAttackSpeed;
        [SerializeField] private float baseRange;
        [SerializeField] private float searchingRange = 60;
        [SerializeField] private LayerMask allyLayer;

        public string CharacterName => characterName;
        public int ID => id;
        public string CombatClass => combatClass;
        public string Role => role;
        public float SearchingRange => searchingRange;
        public LayerMask AllyLayer => allyLayer;
        public LayerMask EnemyLayer => allyLayer.Inverse();

        public Action OnUpdate { get; set; }
        public Action OnIdle { get; set; }
        public Action OnWork { get; set; }
        public Action OnRun { get; set; }
        public Action OnAttack { get; set; }
        public Action OnSkill { get; set; }
        public Action OnLookLeft { get; set; }
        public Action OnLookRight { get; set; }
        // public Func<ICombatTaker> GetCombatTaker { get; set; }
        // public Func<List<ICombatTaker>> GetCombatTakerList { get; set; }

        public float MoveSpeed
        {
            get => baseMoveSpeed;
            set => baseMoveSpeed = value;
        }
        
        public float AttackSpeed
        {
            get => baseAttackSpeed;
            set => baseAttackSpeed = value;
        }

        public float Range
        {
            get => baseRange;
            set => baseRange = value;
        }

        // public ICombatTaker CombatTaker => GetCombatTaker?.Invoke();
        // public List<ICombatTaker> CombatTakerList => GetCombatTakerList?.Invoke();
        public List<GameObject> CharacterSearchedList { get; } = new();
        public List<GameObject> MonsterSearchedList { get; } = new();

        public void Idle() => OnIdle?.Invoke();
        public void Walk() => OnWork?.Invoke();
        public void Run() => OnRun?.Invoke();
        public void Attack() => OnAttack?.Invoke();
        public void Skill() => OnSkill?.Invoke();
        public void LookLeft() => OnLookLeft?.Invoke();
        public void LookRight() => OnLookRight?.Invoke();
        
        
        public void Initialize(string characterName)
        {
            var profile = MainData.GetAdventurerData(characterName);

            this.characterName = profile.Name;
            id = profile.ID;
            combatClass = profile.Job;
            role = profile.Role;

            var classData = MainData.GetCombatClassData(combatClass);

            baseAttackSpeed = classData.AttackSpeed;
            baseRange = classData.Range;
        }


        private void Awake()
        {
            // TODO. OnTest
            Initialize("Kungen");
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        public GameObject Taker => gameObject;

        public void TakeDamage(IDamageProvider damageInfo){}
        public void TakeHeal(IHealProvider healInfo){}
        public void TakeExtra(IExtraProvider extra){}
    }
}
