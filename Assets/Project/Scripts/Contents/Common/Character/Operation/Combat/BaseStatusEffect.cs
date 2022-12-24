using System;
using System.Collections;
using UnityEngine;
using StatusEffectData = MainGame.Data.ContentData.StatusEffectData.StatusEffect;

namespace Common.Character.Operation.Combat
{
    public abstract class BaseStatusEffect
    {
        public Coroutine InvokeRoutine;
        public Action Callback;
        public ICombatProvider ProviderInfo;
        public ICombatTaker TakerInfo;

        public int ID { get; set; }
        public string ActionName { get; set; }
        public bool IsBuff { get; set; }
        public float Duration { get; set; }
        public StatusEffectData BaseData { get; set; }

        public abstract IEnumerator MainAction();
    }
}
