using System;
using System.Collections;
using Core;
using MainGame;
using UnityEngine;

namespace Character.Combat.StatusEffects
{
    public abstract class StatusEffectObject : CombatObject, IStatusEffect, IEditorSetUp
    {
        [SerializeField] protected Sprite icon;
        [SerializeField] protected bool isBuff;
        [SerializeField] protected float duration;
        [SerializeField] protected float combatValue;

        protected Action Callback;
        protected Coroutine RoutineBuffer;
        protected ICombatTaker Taker;

        public Sprite Icon => icon;
        public bool IsBuff => isBuff;
        public virtual float Duration => duration;
        public float CombatValue => combatValue;
        public StatusEffectTable TargetTable { get; set; }


        public override void Initialize(ICombatProvider provider, ICombatTaker taker)
        {
            Provider = provider;
            Taker    = taker;
            
            Taker.TakeStatusEffect(this);
            Callback.AddUniquely(UnregisterTable);
            
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

        private void OnDestroy()
        {
            Callback      = null;
            RoutineBuffer = null;
            Taker         = null;
        }

#if UNITY_EDITOR
        public virtual void SetUp()
        {
            if (actionCode == DataIndex.None) actionCode = name.ToEnum<DataIndex>();
            
            var data = MainData.GetStatusEffect(actionCode);
            
            // icon     = data.~~~
            isBuff      = data.IsBuff;
            duration    = data.Duration;
            combatValue = data.CombatValue;
        }
#endif
    }
}
