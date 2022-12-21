using System;
using System.Collections;
using Core;
using StatusEffectData = MainGame.Data.ContentData.StatusEffectData.StatusEffect;
using UnityEngine;

namespace Common.Character.Operation.StatusEffect
{
    public abstract class BaseStatusEffect
    {
        public Coroutine InvokeRoutine;
        public Action Callback;
        public ICombatProvider ProviderInfo;
        public ICombatTaker TakerInfo;

        public int ID { get; set; }
        public string ActionName { get; set; }
        public float Duration { get; set; }
        public StatusEffectData BaseData { get; set; }

        public abstract IEnumerator MainAction();
    }
}
