using UnityEngine;

namespace Common.StatusEffect
{
    public abstract class StatusEffectComponent : MonoBehaviour, ISequence ,IPoolable<StatusEffectComponent>, IStatusEffect, IEditable
    {
        [SerializeField] protected DataIndex statusCode;
        [SerializeField] protected StatusEffectType type;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected float duration;

        public Pool<StatusEffectComponent> Pool { get; set; }
        public ICombatProvider Provider { get; protected set; }
        public DataIndex ActionCode => statusCode;
        public Sprite Icon => icon;
        public float Duration => duration;
        public FloatEvent ProgressTime { get; } = new();
        
        public ActionTable OnActivated { get; } = new();
        public ActionTable OnCanceled { get; } = new();
        public ActionTable OnCompleted { get; } = new();
        public ActionTable OnEnded { get; } = new();
        
        protected ICombatTaker Taker { get; set; }

        /// <summary>
        /// Scene 시작과 함께 한 번 호출.
        /// </summary>
        public virtual void Initialized(ICombatProvider provider)
        {
            Provider = provider;
            
            ProgressTime.SetClamp(0f, Mathf.Min(duration * 1.5f, 3600));
        }
        

        /// <summary>
        /// 성공적으로 스킬 사용시 호출.
        /// </summary>
        public virtual void Execution(ICombatTaker taker)
        {
            Taker              = taker;
            enabled            = true;
            ProgressTime.Value = duration;

            OnActivated.Invoke();
        }
        

        /// <summary>
        /// 이미 효과를 가진 경우 호출.
        /// </summary>
        public virtual void OnOverride()
        {
            ProgressTime.Value += duration;
        }
        

        /// <summary>
        /// 해제 시 호출. (만료 아님)
        /// </summary>
        public void Cancellation()
        {
            OnCanceled.Invoke();
            
            End();
        }
        
        
        /// <summary>
        /// Scene이 종료되거나, 설정된 Pool 개수를 넘어서 생성된 상태이상효과가 만료될 때 호출
        /// </summary>
        public virtual void Disposed()
        {
            Destroy(gameObject);
        }


        /// <summary>
        /// 성공적으로 만료시 호출
        /// </summary>
        protected void Complete()
        {
            OnCompleted.Invoke();

            End();
        }
        
        
        /// <summary>
        /// 성공 실패와 관계없이 호출
        /// </summary>
        protected void End()
        {
            var targetTable = type == StatusEffectType.Buff
                ? Taker.DynamicStatEntry.BuffTable
                : Taker.DynamicStatEntry.DeBuffTable;
            
            targetTable.Unregister(this);

            OnEnded.Invoke();
            Pool.Release(this);
            enabled = false;
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            var statusEffectData = Database.StatusEffectSheetData(ActionCode);

            duration = statusEffectData.Duration;
            type = statusEffectData.IsBuff
                ? StatusEffectType.Buff
                : StatusEffectType.DeBuff;

            Database.TryGetIcon(ActionCode.ToString(), out icon);
        }
#endif
    }
}
