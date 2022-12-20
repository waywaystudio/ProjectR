using System;
using System.Collections;
using Core;
using StatusEffectData = MainGame.Data.ContentData.StatusEffectData.StatusEffect;
using UnityEngine;

namespace Common.Character.Operation.StatusEffecting
{
    public abstract class BaseStatusEffect
    {
        public Coroutine InvokeRoutine;
        public Action Callback;
        public ICombatProvider ProviderInfo;
        public ICombatTaker TakerInfo;

        public int ID { get; set; }
        public string Name { get; set; }
        public float Duration { get; set; }
        public bool IsBuff { get; set; }
        public StatusEffectData BaseData { get; set; }

        public abstract IEnumerator MainAction();
    }
}
