using System.Collections;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.StatusEffect
{
    public abstract class StatusEffectComponent : MonoBehaviour, IPoolable<StatusEffectComponent>, IStatusEffect, IDataSetUp
    {
        [SerializeField] protected DataIndex statusCode;
        [SerializeField] protected StatusEffectType type;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected float duration;
        
        protected Coroutine StatusEffectRoutine;
        
        public Pool<StatusEffectComponent> Pool { get; set; }
        public ICombatProvider Provider { get; private set; }
        public DataIndex ActionCode => statusCode;
        public StatusEffectType Type => type;
        public Sprite Icon => icon;
        public float Duration => duration;
        public FloatEvent ProcessTime { get; } = new(0f, float.MaxValue);

        [ShowInInspector] public ActionTable<ICombatTaker> OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnDispel { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();

        public void Initialize(ICombatProvider provider)
        {
            Provider = provider;
        }

        public virtual void Active(ICombatTaker taker)
        {
            StartEffectuate(taker);
            
            OnActivated.Invoke(taker);
        }

        public abstract void OnOverride();

        public virtual void Dispel()
        {
            OnDispel.Invoke();
            
            End();
        }


        protected abstract void Init();
        protected abstract IEnumerator Effectuating(ICombatTaker taker);

        protected virtual void Complete()
        {
            OnCompleted.Invoke();
            End();
            
        }

        protected virtual void End()
        {
            StopEffectuate();
            OnEnded.Invoke();
            
            OnActivated.Clear();
            OnDispel.Clear();
            OnCompleted.Clear();
            OnEnded.Clear();
            
            Pool.Release(this);
        }
        

        private void StartEffectuate(ICombatTaker taker)
        {
            StopEffectuate();
            StatusEffectRoutine = StartCoroutine(Effectuating(taker));
        }

        private void StopEffectuate()
        {
            if (StatusEffectRoutine != null)
                StopCoroutine(StatusEffectRoutine);
        }

        protected virtual void Awake()
        {
            ProcessTime.SetClamp(0f, Mathf.Min(duration * 1.5f, 3600));

            Init();
        }
        

        public virtual void SetUp()
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
