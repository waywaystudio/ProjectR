using System.Threading;
using Common;
using Common.StatusEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Ranger.StatusEffects
{
    public class ArcaneArrowStatusEffect : StatusEffect
    {
        [Sirenix.OdinInspector.ShowInInspector]
        private int stack = 1;
        private CancellationTokenSource cts;

        public int Stack => stack;

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(SectionType.Active, "Effectuate", () => Effectuate().Forget())
                           .Add(SectionType.Cancel, "StopEffectuate", () => cts?.Cancel());
        }

        public override void Overriding()
        {
            base.Overriding();

            if (stack >= 6) return;
            stack++;
        }


        private async UniTaskVoid Effectuate()
        {
            cts = new CancellationTokenSource();
            
            while (true)
            {
                ProgressTime.Value -= Time.deltaTime;

                if (ProgressTime.Value <= 0f)
                {
                    break;
                }

                await UniTask.Yield(cts.Token);
            }
            
            SequenceInvoker.Complete();
        }
    }
}
