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
        // TEST
        [SerializeField] private bool isAlive = true;
        [SerializeField] private double hp = 1000;
        //
        
        [SerializeField] private string characterName = string.Empty;
        [SerializeField] private int id;
        [SerializeField] private string combatClass;
        [SerializeField] private float searchingRange = 50f;
        [SerializeField] private LayerMask allyLayer;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private ValueTable maxHp = new();
        [SerializeField] private ValueTable moveSpeed = new();
        [SerializeField] private ValueTable critical = new();
        [SerializeField] private ValueTable haste = new();
        [SerializeField] private ValueTable hit = new();
        [SerializeField] private ValueTable evade = new();
        
        public string CharacterName => characterName ??= "Diablo";
        public int ID => id;
        public string CombatClass => combatClass ??= MainData.GetAdventurerData(CharacterName).Job;
        public float SearchingRange => searchingRange;
        public LayerMask AllyLayer => allyLayer;
        public LayerMask EnemyLayer => enemyLayer;

        public ActionTable OnStart { get; } = new();
        public ActionTable OnUpdate { get; } = new();
        public ActionTable OnIdle { get; } = new();
        public ActionTable<Vector3> OnWalk { get; } = new();
        public ActionTable<Vector3> OnRun { get; } = new();
        public ActionTable<string, Action> OnSkill { get; } = new();
        public ActionTable OnSkillHit { get; } = new(1);

        public FunctionTable<bool> IsReached { get; } = new();
        public FunctionTable<Vector3> Direction { get; } = new();
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public double Hp { get => hp; set => hp = value; }
        
        public ValueTable MaxHp => maxHp;
        public ValueTable MoveSpeed => moveSpeed;
        public ValueTable Critical => critical;
        public ValueTable Haste => haste;
        public ValueTable Hit => hit;
        public ValueTable Evade => evade;

        public List<GameObject> CharacterSearchedList { get; } = new();
        public List<GameObject> MonsterSearchedList { get; } = new();
        public ICombatTaker MainTarget { get; set; }

        public void Idle() => OnIdle?.Invoke();
        public void Walk(Vector3 destination) => OnWalk?.Invoke(destination);
        public void Run(Vector3 destination) => OnRun?.Invoke(destination);
        public void Skill(string skillName, Action callback) => OnSkill?.Invoke(skillName, callback);
        public void SkillHit() => OnSkillHit?.Invoke();

        public void Initialize(string character)
        {
            var profile = MainData.GetAdventurerData(character);

            characterName = profile.Name;
            id = profile.ID;
            combatClass = profile.Job;

            var classData = MainData.GetCombatClassData(combatClass);
        }

        protected void Start()
        {
            Initialize(characterName ?? "Diablo");
            
            OnStart?.Invoke();
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
        }
        public virtual void TakeExtra(IExtraProvider extra) {}

#if UNITY_EDITOR
        [Button]
        private void Initialize() => Initialize(characterName);
#endif
    }
}
