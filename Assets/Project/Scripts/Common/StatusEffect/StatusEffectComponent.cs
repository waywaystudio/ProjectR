using Common.Execution;
using UnityEngine;

namespace Common.StatusEffect
{
    public abstract class StatusEffectComponent : MonoBehaviour, IStatusEffect, IEditable
                                                  , ISections<ICombatTaker>
    {
        [SerializeField] protected Executor executor;
        [SerializeField] private StatusEffectSequencer sequencer;
        
        [SerializeField] protected DataIndex statusCode;
        [SerializeField] protected StatusEffectType type;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected float duration;

        public ICombatProvider Provider { get; protected set; }
        public DataIndex DataIndex => statusCode;
        public Sprite Icon => icon;
        public float Duration => duration;
        public FloatEvent ProgressTime { get; } = new();
        public StatusEffectSequenceBuilder SequenceBuilder { get; } = new();
        public StatusEffectSequenceInvoker SequenceInvoker { get; } = new();
        
        public ActionTable<ICombatTaker> ActiveParamAction => sequencer.ActiveParamAction;
        public ConditionTable Condition => sequencer.Condition;
        public ActionTable ActiveAction => sequencer.ActiveAction;
        public ActionTable CancelAction => sequencer.CancelAction;
        public ActionTable CompleteAction => sequencer.CompleteAction;
        public ActionTable EndAction => sequencer.EndAction;
        public ActionTable ExecuteAction => sequencer.ExecuteAction;

        protected ICombatTaker Taker { get; set; }
        

        /// <summary>
        /// Scene 시작과 함께 한 번 호출.
        /// 보통 SkillSequence에 StatusCompletion으로 부터 호출 됨.
        /// </summary>
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;
            ProgressTime.SetClamp(0f, Mathf.Min(duration * 1.5f, 3600));

            SequenceInvoker.Initialize(sequencer);
            SequenceBuilder.Initialize(sequencer)
                           .Add(SectionType.End,"UnregisterTable", UnregisterTable)
                           .Add(SectionType.End,"DisableComponent", () => enabled = false)
                           .Add(SectionType.End,"DeActiveGameObject", () => gameObject.SetActive(false));
        }

        /// <summary>
        /// 성공적으로 스킬 사용시 호출.
        /// </summary>
        public virtual void Activate(ICombatTaker taker)
        {
            Taker              = taker;
            enabled            = true;
            ProgressTime.Value = duration;

            gameObject.SetActive(true);
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
        public void Cancel() => SequenceInvoker.Cancel();


        /// <summary>
        /// Scene이 종료되거나, 설정된 Pool 개수를 넘어서 생성된 상태이상효과가 만료될 때 호출
        /// </summary>
        public virtual void Dispose()
        {
            sequencer.Clear();
            
            Destroy(gameObject);
        }


        private void UnregisterTable()
        {
            var targetTable = type == StatusEffectType.Buff
                ? Taker.DynamicStatEntry.BuffTable
                : Taker.DynamicStatEntry.DeBuffTable;

            targetTable.Unregister(this);
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            executor = GetComponentInChildren<Executor>();
            
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

/* Skill과 다르게 Execution이 항상 내부에서 이루어진다.
 * SE가 확장된다면, Skill Execution 구조를 참고하자. */
 