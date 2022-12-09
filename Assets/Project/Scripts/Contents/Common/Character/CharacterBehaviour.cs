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
        [SerializeField] private float searchingRange;
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
        public Action OnAttackHit { get; set; }
        public Action OnFootstep { get; set; }
        public Action OnSkill { get; set; }
        public Action OnSkillHit { get; set; }
        public Action OnLookLeft { get; set; }
        public Action OnLookRight { get; set; }

        public float MoveSpeed { get => baseMoveSpeed; set => baseMoveSpeed = value; }
        public float AttackSpeed { get => baseAttackSpeed; set => baseAttackSpeed = value; } // 이게 무슨 의미가 있을라나...전반적인 가속 계수가 되려나

        public List<GameObject> CharacterSearchedList { get; } = new();
        public List<GameObject> MonsterSearchedList { get; } = new();

        public void Idle() => OnIdle?.Invoke();
        public void Walk() => OnWork?.Invoke();
        public void Run() => OnRun?.Invoke();
        public void Attack() => OnAttack?.Invoke();
        public void Skill() => OnSkill?.Invoke();
        public void AttackHit() => OnAttackHit?.Invoke();
        public void Footstep() => OnFootstep?.Invoke();
        public void SkillHit() => OnSkillHit?.Invoke();
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
