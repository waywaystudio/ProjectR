using System;
using System.Collections;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;
using StatusEffectData = MainGame.Data.ContentData.StatusEffectData.StatusEffect;

namespace Common.Character.Operation.Combating
{
    [Serializable]
    public abstract class StatusEffect
    {
        protected StatusEffect() : this(nameof(StatusEffect)) {}
        protected StatusEffect(string name)
        {
            Name = name;
            StatusEffectData = MainData.GetStatusEffectData(Name);
            ID = StatusEffectData.ID;
            Duration = StatusEffectData.Duration;
            IsBuff = StatusEffectData.IsBuff;
        }

        public Coroutine InvokeRoutine;

        public int ID { get; set; }
        public string Name { get; set; }
        protected float Duration;
        protected bool IsBuff;
        
        protected StatusEffectData StatusEffectData;

        public abstract IEnumerator Invoke();

#if UNITY_EDITOR
        [Button(ButtonSizes.Large, Icon = SdfIconType.ArrowRepeat)]
        private void GetDataFromDB()
        {
            if (Name.IsNullOrEmpty())
            {
                Debug.LogError("Status Effect Name Required!");
                return;
            }
            
            StatusEffectData = MainData.GetStatusEffectData(Name);
            ID = StatusEffectData.ID;
            Duration = StatusEffectData.Duration;
            IsBuff = StatusEffectData.IsBuff;
        }
#endif
    }
}
