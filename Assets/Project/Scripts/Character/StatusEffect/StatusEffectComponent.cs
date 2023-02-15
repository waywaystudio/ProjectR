using System;
using System.Collections;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Character.StatusEffect
{
    public abstract class StatusEffectComponent : MonoBehaviour, IStatusEffect
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
        public Sprite Icon => icon;
        public float Duration => duration;
        public StatusEffectType StatusEffectType => type;
        public StatusEffectTable TargetTable { get; set; }

        [ShowInInspector] public ActionTable<ICombatTaker> OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnDispel { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();
        
        public void SetPool(IObjectPool<StatusEffectComponent> pool) => this.pool = pool;

        public void Initialize(ICombatProvider provider)
        {
            Provider = provider;
        }

        public void Active(ICombatTaker taker)
        {
            OnActivated.Invoke(taker);
        }
        
        public void DeActive()
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

        protected void End()
        {
            OnEnded.Invoke();
        }
       
        private void RegisterTable(ICombatEntity taker)
        {
            TargetTable = type switch
            {
                StatusEffectType.Buff => taker.DynamicStatEntry.BuffTable,
                StatusEffectType.DeBuff => taker.DynamicStatEntry.DeBuffTable,
                StatusEffectType.None => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            TargetTable.Register(this);
        }
        
        private void UnregisterTable()
        {
            if (TargetTable.HasElement())
            {
                TargetTable.Unregister(this);
                TargetTable = null;
            }
        }
        
        private void Effectuate(ICombatTaker taker)
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
            
            OnActivated.Register("EffectRoutine", Effectuate);
            OnActivated.Register("RegisterTable", RegisterTable);
            
            OnEnded.Register("StopEffectRoutine", StopEffectuate);
            OnEnded.Register("UnregisterTable", UnregisterTable);
            OnEnded.Register("ReleasePool", () => pool.Release(this));
            
            Init();
        }

        public void SetUp() { }
    }
}
