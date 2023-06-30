using Common.Execution;
using UnityEngine;

namespace Common.StatusEffects
{
    public abstract class StatusEffect : MonoBehaviour, IOriginalProvider, IHasSequencer, IEditable
    {
        [SerializeField] protected Executor executor;
        [SerializeField] private Sequencer<ICombatTaker> sequencer;
        
        [SerializeField] protected DataIndex statusCode;
        [SerializeField] protected StatusEffectType type;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected float duration;

        public ICombatProvider Provider { get; protected set; }
        public DataIndex DataIndex => statusCode;
        public StatusEffectType Type => type;
        public FloatEvent ProgressTime { get; } = new();
        public Sequencer Sequencer => sequencer;
        public Sprite Icon => icon;
        public float Duration => duration;

        protected SequenceBuilder<ICombatTaker> SequenceBuilder { get; private set; }
        protected StatusEffectSequenceInvoker SequenceInvoker { get; private set; }
        protected ICombatTaker Taker { get; set; }
        

        /// <summary>
        /// Scene 시작과 함께 한 번 호출.
        /// 보통 SkillSequence에 StatusCompletion으로 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;
            ProgressTime.SetClamp(0f, Mathf.Min(duration * 1.5f, 3600));

            SequenceInvoker = new StatusEffectSequenceInvoker(sequencer);
            SequenceBuilder = new SequenceBuilder<ICombatTaker>(sequencer);

            SequenceBuilder.Add(SectionType.Active, "SetAbleToTrue", () => enabled              = true)
                           .Add(SectionType.Active, "SetProgressTime", () => ProgressTime.Value = duration)
                           .Add(SectionType.Active, "SetObjectActive", () => gameObject.SetActive(true))
                           .Add(SectionType.Active, "AddStatusEffectTable", AddTable)
                           .Add(SectionType.End, "DisableComponent", () => enabled = false)
                           .Add(SectionType.End, "DeActiveGameObject", () => gameObject.SetActive(false))
                           .Add(SectionType.End, "UnregisterTable", RemoveTable);
        }

        /// <summary>
        /// 성공적으로 스킬 사용시 호출.
        /// </summary>
        public void Activate(ICombatTaker taker)
        {
            Taker = taker;
            
            SequenceInvoker.Active(taker);
        }

        /// <summary>
        /// 이미 효과를 가진 경우 호출.
        /// </summary>
        public virtual void Overriding()
        {
            ProgressTime.Value += duration;
        }

        /// <summary>
        /// 해제 시 호출 == Dispel. (만료 아님)
        /// </summary>
        public virtual void Dispel() => SequenceInvoker.Cancel();


        private void AddTable() => Taker.DynamicStatEntry.StatusEffectTable.Add(DataIndex, this);
        private void RemoveTable() => Taker.DynamicStatEntry.StatusEffectTable.Remove(DataIndex);
        private void OnDestroy()
        {
            sequencer.Clear();
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