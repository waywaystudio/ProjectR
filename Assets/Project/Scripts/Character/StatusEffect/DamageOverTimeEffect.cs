using System.Collections;
using Core;
using UnityEngine;

namespace Character.StatusEffect
{
    // TODO. 현재 구조에서 틱 데미지가 끝나고 큰 데미지를 주기 힘들다.
    public class DamageOverTimeEffect : StatusEffectComponent, ICombatTable
    {
        [SerializeField] private float interval;
        [SerializeField] private PowerValue tickPowerValue;
        
        public StatTable StatTable { get; } = new();
        
        
        public override void OnOverride()
        {
            ProcessTime.Value += duration;
        }
        

        protected override void Init() { }
        protected override void End()
        {
            ProcessTime.Value = 0f;
            
            base.End();
        }

        protected override IEnumerator Effectuating(ICombatTaker taker)
        {
            var hastedTick = interval * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);

            ProcessTime.Value = duration;

            while (ProcessTime.Value > 0)
            {
                var tickBuffer = hastedTick;

                while (tickBuffer > 0f)
                {
                    ProcessTime.Value -= Time.deltaTime;
                    tickBuffer        -= Time.deltaTime;
                    yield return null;
                }
                
                UpdateDoT();
                taker.TakeDamage(this);
            }

            Complete();
        }
        

        protected void UpdateDoT()
        {
            StatTable.Clear();
            StatTable.Register(ActionCode, tickPowerValue);
            StatTable.UnionWith(Provider.StatTable);
        }
    }
}
