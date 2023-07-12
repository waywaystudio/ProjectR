using Common.Effects;
using Common.Execution;
using UnityEngine;

namespace Common.StatusEffects
{
    public abstract class StatusEffect : MonoBehaviour, IActionSender, IHasTaker, ICombatSequence, IEditable
    {
        [SerializeField] protected HitExecutor hitExecutor;
        [SerializeField] protected Effector effector;
        [SerializeField] protected DataIndex statusCode;
        [SerializeField] protected StatusEffectType type;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected float duration;

        public ICombatProvider Provider { get; protected set; }
        public ICombatTaker Taker { get; set; }
        public DataIndex DataIndex => statusCode;
        public StatusEffectType Type => type;
        public FloatEvent ProgressTime { get; } = new();
        public Sprite Icon => icon;
        public float Duration => duration;
        
        public CombatSequence Sequence { get; } = new();
        public CombatSequenceInvoker Invoker { get; private set; }
        protected CombatSequenceBuilder Builder { get; private set; }
        

        /// <summary>
        /// Scene 시작과 함께 한 번 호출.
        /// 보통 SkillSequence에 StatusEffectExecutor로 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;
            Invoker  = new CombatSequenceInvoker(Sequence);
            Builder  = new CombatSequenceBuilder(Sequence);
            Builder
                .Add(Section.Active, "ResetProgressTime", () => ProgressTime.Value = duration)
                .Add(Section.Active, "AddStatusEffectTable", AddTable)
                .Add(Section.Override, "ProlongDuration", ProlongDuration)
                .Add(Section.End, "UnregisterTable", RemoveTable);
            
            hitExecutor.Initialize(Sequence, this);
            effector.Initialize(Sequence, this);
        }
        

        /// <summary>
        /// 성공적으로 스킬 사용시 호출.
        /// </summary>
        public void Activate(ICombatTaker taker)
        {
            Taker = taker;
            Invoker.Active();
        }

        public void Dispel() => Invoker.Cancel();


        protected virtual void Dispose()
        {
            /* Annotation
             * SKillComponent와 다르게 Pooling 기반 Object는 Dispose에서 Invoker.End실행 불가.
             * Pool을 가진 Object가 먼저 파괴되었을 수 있음
             * Invoker.End(); */
            Sequence.Clear();
        }


        private void AddTable() => Taker.StatusEffectTable.Add(DataIndex, this);
        private void RemoveTable() => Taker.StatusEffectTable.Remove(DataIndex);

        private void ProlongDuration()
        {
            var overrideValue = ProgressTime.Value + duration;
            
            ProgressTime.Value = Mathf.Clamp(overrideValue, 0, duration * 1.5f);
        }
        
        private void OnDestroy()
        {
            Dispose();
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            hitExecutor.GetExecutionInEditor(transform);
            effector.GetEffectsInEditor(transform);

            var statusEffectData = Database.StatusEffectSheetData(DataIndex);

            icon     = Database.SpellSpriteData.Get(DataIndex);
            duration = statusEffectData.Duration;
            type = statusEffectData.IsBuff
                ? StatusEffectType.Buff
                : StatusEffectType.DeBuff;
        }
#endif
    }
}