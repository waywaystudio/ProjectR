using System.Threading;
using Common;
using Common.StatusEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Priest.StatusEffects
{
    public class LightWeaverStatusEffect : StatusEffect
    {
        private CancellationTokenSource cts;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder
                .Add(SectionType.Active, "Effectuate", () => Effectuate().Forget())
                .Add(SectionType.End, "StopEffectuate", () => cts?.Cancel());
        }
        
        
        private async UniTaskVoid Effectuate()
        {
            cts = new CancellationTokenSource();

            while (ProgressTime.Value > 0)
            {
                ProgressTime.Value -= Time.deltaTime;

                await UniTask.Yield(cts.Token);
            }
            
            SequenceInvoker.Complete();
        }
    }
}
