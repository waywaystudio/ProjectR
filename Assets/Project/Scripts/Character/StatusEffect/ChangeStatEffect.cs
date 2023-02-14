using System.Collections;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.StatusEffect
{
    public class ChangeStatEffect : StatusEffectComponent
    {
        [SerializeField] private ArmorValue armorValue;

        [ShowInInspector] private FloatEvent Progress { get; } = new(0f, float.MaxValue);

        protected override void Init() { }
        protected override IEnumerator Effectuating(ICombatTaker taker)
        {
            var durationBuffer = duration;
            Progress.Value = 0f;
            
            taker.StatTable.Register(ActionCode, armorValue);

            while (durationBuffer > 0f)
            {
                Progress.Value += StatusEffectTick;
                durationBuffer -= StatusEffectTick;
                
                yield return null;
            }

            Complete();
        }
    }
}
