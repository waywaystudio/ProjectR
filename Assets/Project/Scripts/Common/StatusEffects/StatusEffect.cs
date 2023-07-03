using Common.Execution;
using UnityEngine;

namespace Common.StatusEffects
{
    public abstract class StatusEffect : MonoBehaviour, IOriginalProvider, IHasSequencer, IEditable
    {
        [SerializeField] protected Executor executor;
        [SerializeField] protected DataIndex statusCode;
        [SerializeField] protected StatusEffectType type;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected float duration;

        public float Duration => duration;
        public FloatEvent ProgressTime { get; } = new();
        public ICombatProvider Provider { get; protected set; }
        public DataIndex DataIndex => statusCode;
        public StatusEffectType Type => type;
        public Sequencer Sequencer { get; } = new();
        public Sprite Icon => icon;
        public StatusEffectSequenceInvoker Invoker { get; private set; }
        protected SequenceBuilder Builder { get; private set; }
        protected ICombatTaker Taker { get; set; }
        

        /// <summary>
        /// Scene 시작과 함께 한 번 호출.
        /// 보통 SkillSequence에 StatusEffectExecutor로 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;
            Invoker  = new StatusEffectSequenceInvoker(Sequencer);
            Builder  = new SequenceBuilder(Sequencer);

            Builder
                .Add(SectionType.Active, "ResetProgressTime", () => ProgressTime.Value = duration)
                .Add(SectionType.Active, "AddStatusEffectTable", AddTable)
                .Add(SectionType.Override, "ProlongDuration", ProlongDuration)
                .Add(SectionType.End, "UnregisterTable", RemoveTable);
        }
        

        /// <summary>
        /// 성공적으로 스킬 사용시 호출.
        /// </summary>
        public void Activate(ICombatTaker taker)
        {
            Taker = taker;
            Invoker.Active();
        }


        /// <summary>
        /// 해제 시 호출 == Dispel. (만료 아님)
        /// </summary>
        public void Dispel() => Invoker.Cancel();


        private void AddTable() => Taker.StatusEffectTable.Add(DataIndex, this);
        private void RemoveTable() => Taker.StatusEffectTable.Remove(DataIndex);

        private void ProlongDuration()
        {
            var overrideValue = ProgressTime.Value + duration;
            
            ProgressTime.Value = Mathf.Clamp(overrideValue, 0, duration * 1.5f);
        }
        
        private void OnDestroy()
        {
            Sequencer.Clear();
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            executor.EditorGetExecutions(gameObject);
            
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