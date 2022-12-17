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
        [SerializeField] private FloatTable moveSpeed = new();
        [SerializeField] private FloatTable critical = new();
        [SerializeField] private FloatTable haste = new();
        [SerializeField] private FloatTable hit = new();
        [SerializeField] private FloatTable evade = new();
        
        public string CharacterName => characterName ??= "Diablo";
        public int ID => id;
        public string CombatClass => combatClass ??= MainData.GetAdventurerData(CharacterName).Job;
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
        public FunctionTable<bool> IsReached { get; } = new();
        public FunctionTable<Vector3> Direction { get; } = new();
        public DoubleTable MaxHp => maxHp;
        public FloatTable MoveSpeed => moveSpeed;
        public FloatTable Critical => critical;
        public FloatTable Haste => haste;
        public FloatTable Hit => hit;
        public FloatTable Evade => evade;

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
            combatClass = profile.Job;
        }

        protected virtual void Start()
        {
            Initialize(CharacterName);
            
            OnStart?.Invoke();
        }

        protected void Update() => OnUpdate?.Invoke();

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
