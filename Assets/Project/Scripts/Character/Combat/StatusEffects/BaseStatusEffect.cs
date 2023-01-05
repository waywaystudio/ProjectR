using System;
using System.Collections;
using UnityEngine;
using StatusEffectData = MainGame.Data.ContentData.StatusEffectData.StatusEffect;

namespace Character.Combat.StatusEffects
{
    public abstract class BaseStatusEffect
    {
        public Coroutine InvokeRoutine;
        public Action Callback;
        public ICombatTaker TakerInfo;

        public IDCode ActionCode { get; set; }
        public bool IsBuff { get; set; }
        public float Duration { get; set; }
        public float CombatValue { get; set; }
        public ICombatProvider Provider { get; set; }

        public abstract IEnumerator MainAction();
    }
}
