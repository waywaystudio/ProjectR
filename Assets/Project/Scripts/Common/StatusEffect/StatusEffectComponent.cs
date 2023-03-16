using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.StatusEffect
{
    public abstract class StatusEffectComponent : MonoBehaviour, ISequence ,IPoolable<StatusEffectComponent>, IStatusEffect, IEditable
    {
        [SerializeField] protected DataIndex statusCode;
        [SerializeField] protected StatusEffectType type;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected float duration;
        
        protected Coroutine StatusEffectRoutine;
        
        public Pool<StatusEffectComponent> Pool { get; set; }
        
        public ICombatProvider Provider { get; private set; }
        public ICombatTaker Taker { get; private set; }
        public DataIndex ActionCode => statusCode;
        public StatusEffectType Type => type;
        public Sprite Icon => icon;
        public float Duration => duration;
        public FloatEvent ProgressTime { get; } = new();

        [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnCanceled { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();


        public virtual void Active(ICombatProvider provider, ICombatTaker taker)
        {
            Provider = provider;
            Taker    = taker;
            
            StartEffectuate();
            
            OnActivated.Invoke();
        }

        public abstract void OnOverride();

        public void Cancel()
        {
            OnCanceled.Invoke();
            
            End();
        }
        

        protected abstract IEnumerator Effectuating();

        protected void Complete()
        {
            OnCompleted.Invoke();
            
            End();
        }
        
        protected void End()
        {
            OnEnded.Invoke();
        }
        

        private void StartEffectuate()
        {
            StopEffectuate();
            StatusEffectRoutine = StartCoroutine(Effectuating());
        }

        private void StopEffectuate()
        {
            if (StatusEffectRoutine != null)
                StopCoroutine(StatusEffectRoutine);
        }

        private void UnregisterTable()
        {
            var targetTable = Type == StatusEffectType.Buff
                ? Taker.DynamicStatEntry.BuffTable
                : Taker.DynamicStatEntry.DeBuffTable;
            
            targetTable.Unregister(this);
        }

        private void Awake()
        {
            ProgressTime.SetClamp(0f, Mathf.Min(duration * 1.5f, 3600));

            OnEnded.Register("StopEffectuate", StopEffectuate);
            OnEnded.Register("UnregisterTable", UnregisterTable);
            OnEnded.Register("ReleasePool", () => Pool.Release(this));
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
