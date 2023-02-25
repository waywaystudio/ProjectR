using System.Collections;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.StatusEffect
{
    public abstract class StatusEffectComponent : MonoBehaviour, ISequence ,IPoolable<StatusEffectComponent>, IStatusEffect, IEditable
    {
        [SerializeField] protected DataIndex statusCode;
        [SerializeField] protected StatusEffectType type;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected float duration;
        
        protected Coroutine StatusEffectRoutine;
        protected bool IsInitialized;
        
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
            Initialize(provider);

            Taker    = taker;
            OnActivated.Invoke();
        }

        public abstract void OnOverride();
        public virtual void Cancel() => OnCanceled.Invoke();

        protected abstract IEnumerator Effectuating();
        protected virtual void Complete() => OnCompleted.Invoke();
        protected virtual void End() => OnEnded.Invoke();
        protected virtual void Initialize(ICombatProvider provider)
        {
            if (IsInitialized) return;
            
            IsInitialized = true;
            
            Provider = provider;
            ProgressTime.SetClamp(0f, Mathf.Min(duration * 1.5f, 3600));
            
            OnActivated.Register("StartEffectuate", StartEffectuate);
            
            OnCanceled.Register("End", End);
            
            OnCompleted.Register("End", End);
            
            OnEnded.Register("StopEffectuate", StopEffectuate);
            OnEnded.Register("UnregisterTable", UnregisterTable);
            OnEnded.Register("ReleasePool", () => Pool.Release(this));
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


        public virtual void EditorSetUp()
        {
            var statusEffectData = MainData.StatusEffectSheetData(ActionCode);

            duration = statusEffectData.Duration;
            type = statusEffectData.IsBuff
                ? StatusEffectType.Buff
                : StatusEffectType.DeBuff;

            MainData.TryGetIcon(ActionCode.ToString(), out icon);
        }
    }
}
