using System.Collections;
using Character.Actions;
using Character.Skill;
using Core;
using UnityEngine;

namespace Character.StatusEffect
{
    // TODO. 현재 구조에서 틱 데미지가 끝나고 큰 데미지를 주기 힘들다.
    public class DamageOverTimeEffect : StatusEffectComponent
    {
        [SerializeField] protected ValueCompletion tickPower;
        [SerializeField] protected float interval;
        
        public override void Active(ICombatProvider provider, ICombatTaker taker)
        {
            base.Active(provider, taker);
            
            tickPower.Initialize(provider, ActionCode);
        }
        
        public override void OnOverride()
        {
            ProgressTime.Value += duration;
        }

        protected override IEnumerator Effectuating()
        {
            var hastedTick = interval * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);

            ProgressTime.Value = duration;

            while (ProgressTime.Value > 0)
            {
                var tickBuffer = hastedTick;

                while (tickBuffer > 0f)
                {
                    ProgressTime.Value -= Time.deltaTime;
                    tickBuffer         -= Time.deltaTime;
                    
                    yield return null;
                }
                
                tickPower.Damage(Taker);
            }

            Complete();
        }
    }
}
