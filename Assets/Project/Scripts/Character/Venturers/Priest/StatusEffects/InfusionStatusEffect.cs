using System.Threading;
using Common;
using Common.StatusEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Priest.StatusEffects
{
    public class InfusionStatusEffect : StatusEffect
    {
        [SerializeField] protected float interval = 2f;
        [SerializeField] private StatEntity speedUpStat;
        [SerializeField] private StatEntity criticalChanceStat;
        [SerializeField] private StatEntity hasteStat;
        
        private CancellationTokenSource cts;
        private float hasteWeight;
        private float tickBuffer;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder
                .Add(SectionType.Active, "SetHasteWeightAndTickBuffer", SetHasteWeightAndTickBuffer)
                .Add(SectionType.Active, "Effectuate", () => Effectuate().Forget())
                .Add(SectionType.Active, "ProvideBuff", ProvideBuff)
                .Add(SectionType.End, "StopEffectuate", () => cts?.Cancel())
                .Add(SectionType.End, "RemoveBuffStat", EndBuff);
        }


        private void SetHasteWeightAndTickBuffer()
        {
            hasteWeight = tickBuffer =
                interval * CombatFormula.GetHasteValue(Provider.StatTable.Haste);
        }

        private async UniTaskVoid Effectuate()
        {
            cts = new CancellationTokenSource();

            while (ProgressTime.Value > 0)
            {
                if (tickBuffer > 0f)
                {
                    ProgressTime.Value -= Time.deltaTime;
                    tickBuffer         -= Time.deltaTime;
                }
                else
                {
                    executor.Execute(Taker);
                    tickBuffer = hasteWeight;
                }

                await UniTask.Yield(cts.Token);
            }
            
            SequenceInvoker.Complete();
        }

        private void ProvideBuff()
        {
            Taker.StatTable.Add(speedUpStat);

            if (Provider.DynamicStatEntry.StatusEffectTable.ContainsKey(DataIndex.LightWeaverStatusEffect))
            {
                Taker.StatTable.Add(criticalChanceStat);
                Taker.StatTable.Add(hasteStat);
            }
        }

        private void EndBuff()
        {
            Taker.StatTable.Remove(speedUpStat);
            Taker.StatTable.Remove(criticalChanceStat);
            Taker.StatTable.Remove(hasteStat);
        }
    }
}