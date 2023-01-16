using System;
using System.Collections;
using Core;
using MainGame;
using UnityEngine;

namespace Character.Combat.StatusEffects
{
    public abstract class StatusEffectObject : CombatObject, IStatusEffect
    {
        [SerializeField] protected Sprite icon;
        [SerializeField] protected bool isBuff;
        [SerializeField] protected float duration;
        [SerializeField] protected float combatValue;

        protected ICombatTaker Taker;
        protected Action Callback;
        protected Coroutine RoutineBuffer;
        protected WaitForSeconds WaitBuffer;

        public Sprite Icon => icon;
        public bool IsBuff => isBuff;
        public virtual float Duration => duration;
        public float CombatValue => combatValue;
        public StatusEffectTable TargetTable { get; set; }


        public virtual void Effectuate(ICombatProvider provider, ICombatTaker taker)
        {
            Provider = provider;
            Taker    = taker;
            
            Taker.TakeStatusEffect(this);
            RoutineBuffer = StartCoroutine(Initiate());
        }


        protected abstract IEnumerator Initiate();
        
        private void UnregisterTable()
        {
            if (TargetTable.HasElement())
            {
                TargetTable.Unregister(this);
                TargetTable = null;
            }
        }

        protected void Awake()
        {
            Callback.AddUniquely(UnregisterTable);
        }

        private void OnDestroy()
        {
            Callback      = null;
            RoutineBuffer = null;
            Taker         = null;
        }

#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();
            
            var data = MainData.GetStatusEffect(actionCode);
            
            GetComponents<CombatModule>().ForEach(x => ModuleUtility.SetStatusEffectModule(data, x));

            // icon     = data.~~~
            isBuff      = data.IsBuff;
            duration    = data.Duration;
            combatValue = data.CombatValue;
        }
#endif
    }
}
