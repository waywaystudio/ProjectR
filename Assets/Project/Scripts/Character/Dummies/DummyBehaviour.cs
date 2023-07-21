using System;
using Common;
using Common.Characters;
using Common.Characters.Behaviours;
using Common.StatusEffects;
using UnityEngine;

namespace Character.Dummies
{
    public class DummyBehaviour : MonoBehaviour, ICombatTaker, IEditable
    {
        [SerializeField] private bool isSelfInitialize;
        [SerializeField] private string vummyName;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] protected DummyCombatStatusData combatData;
        [SerializeField] protected CharacterCombatStatus combatStatus;
        [SerializeField] protected CharacterPreposition prePosition;
        [SerializeField] protected DeadBehaviour deadBehaviour;

        public string Name => vummyName;
        public Vector3 Position => transform.position;
        public Vector3 Forward => transform.forward;
        public CharacterMask CombatClass => CharacterMask.Vummy;
        public SpriteRenderer SpriteRenderer => spriteRenderer;
        
        public AliveValue Alive => combatStatus.Alive;
        public HpValue Hp => combatStatus.Hp;
        public ResourceValue Resource => combatStatus.Resource;
        public ShieldValue Shield => combatStatus.Shield;
        public StatTable StatTable => combatStatus.StatTable;
        public StatusEffectTable StatusEffectTable => combatStatus.StatusEffectTable;
        public Transform CombatStatusHierarchy => combatStatus.transform;
        public ActionTable<CombatEntity> OnCombatTaken { get; } = new();

        public ActionMask BehaviourMask => CurrentBehaviour is null ? ActionMask.None : CurrentBehaviour.BehaviourMask;
        public IActionBehaviour CurrentBehaviour { get; set; }
        public StopBehaviour StopBehaviour { get; } = null;
        public RunBehaviour RunBehaviour { get; } = null;
        public StunBehaviour StunBehaviour { get; } = null;
        public KnockBackBehaviour KnockBackBehaviour { get; } = null;
        public DrawBehaviour DrawBehaviour { get; } = null;
        public DeadBehaviour DeadBehaviour => deadBehaviour;

        public void TakeStatusEffect(StatusEffect effect) => StatusEffectTable[effect.DataIndex].Activate(this);
        public void DispelStatusEffect(DataIndex effectIndex) => StatusEffectTable[effectIndex]?.Dispel();
        public Transform Preposition(PrepositionType type) => prePosition.Get(type);

        public void Initialize()
        {
            combatData.Initialize();
            combatStatus.Initialize(combatData.StatTable);
        }


        private void Awake()
        {
            if (isSelfInitialize)
            {
                Initialize();
            }
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            combatStatus   = GetComponentInChildren<CharacterCombatStatus>();
            prePosition    = GetComponentInChildren<CharacterPreposition>();
        }
#endif
    }
}
