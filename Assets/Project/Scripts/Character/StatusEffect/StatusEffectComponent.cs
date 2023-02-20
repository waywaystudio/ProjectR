using System.Collections;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Character.StatusEffect
{
    public abstract class StatusEffectComponent : MonoBehaviour, IStatusEffect, IDataSetUp
    {
        [SerializeField] protected DataIndex statusCode;
        [SerializeField] protected StatusEffectType type;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected float duration;
        
        protected Coroutine StatusEffectRoutine;
        protected float StatusEffectTick;
        private IObjectPool<StatusEffectComponent> pool;

        public ICombatProvider Provider { get; private set; }
        public DataIndex ActionCode => statusCode;
        public StatusEffectType Type => type;
        public Sprite Icon => icon;
        public float Duration => duration;
        
        [ShowInInspector]
        public FloatEvent ProcessTime { get; } = new(0f, float.MaxValue);

        [ShowInInspector] public ActionTable<ICombatTaker> OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnDispel { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();


        public void SetPool(IObjectPool<StatusEffectComponent> pool) => this.pool = pool;

        public void Initialize(ICombatProvider provider)
        {
            Provider = provider;
            
            OnActivated.Clear();
            OnDispel.Clear();
            OnCompleted.Clear();
            OnEnded.Clear();
        }

        public void Active(ICombatTaker taker)
        {
            StartEffectuate(taker);
            
            OnActivated.Invoke(taker);
        }

        public abstract void OnOverride();

        public void Dispel()
        {
            OnDispel.Invoke();
            
            End();
        }


        protected abstract void Init();
        protected abstract IEnumerator Effectuating(ICombatTaker taker);

        protected void Complete()
        {
            OnCompleted.Invoke();
            End();
            
        }

        protected virtual void End()
        {
            StopEffectuate();
            OnEnded.Invoke();
            
            pool.Release(this);
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
            StatusEffectTick = Time.deltaTime;
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
