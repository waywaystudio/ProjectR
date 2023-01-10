using System;
using System.Collections;
using Core;
using MainGame;
using UnityEngine;

namespace Character.Combat.StatusEffects
{
    public abstract class StatusEffectObject : MonoBehaviour, IStatusEffect, IEditorSetUp
    {
        [SerializeField] protected DataIndex statusEffectID;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected bool isBuff;
        [SerializeField] protected float duration;
        [SerializeField] protected float combatValue;
        
        protected int InstanceID;
        protected Action Callback;
        protected Coroutine RoutineBuffer;
        protected ICombatTaker Taker;

        public Sprite Icon => icon;
        public bool IsBuff => isBuff;
        public virtual float Duration => duration;
        public float CombatValue => combatValue;
        public StatusEffectTable TargetTable { get; set; }
        public DataIndex ActionCode { get => statusEffectID; set => statusEffectID = value; }
        public ICombatProvider Provider { get; set; }


        public void Register(ICombatTaker taker)
        {
            Taker = taker;
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
        
        
        protected virtual void Awake()
        {
            Provider   =  GetComponentInParent<ICombatProvider>();
            Callback   += UnregisterTable;
            InstanceID =  GetInstanceID();
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
            if (statusEffectID == DataIndex.None) statusEffectID = name.ToEnum<DataIndex>();
            
            var data = MainData.GetStatusEffect(statusEffectID);
            
            // icon     = data.~~~
            isBuff      = data.IsBuff;
            duration    = data.Duration;
            combatValue = data.CombatValue;
        }
#endif
    }
}
