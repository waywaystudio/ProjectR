using System.Threading;
using Common.Skills;
using Cysharp.Threading.Tasks;

namespace Character.Venturers.Warrior.Skills
{
    public class WarriorsFury : SkillComponent
    {
        private CancellationTokenSource cts;
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddCondition("FullResource", FullResource)
                .Add(Section.Active, "ActiveBuff", AddEffect)
                .Add(Section.Active, "CompleteNextFrame", () => CompleteOnNextFrame().Forget())
                .Add(Section.End, "StopNextFramer", StopNextFramer)
                ;
        }
        
        protected override void Dispose()
        {
            base.Dispose();

            StopNextFramer();
        }


        private bool FullResource()
        {
            return Cb.Resource.Value >= 100f;
        }
        
        private void AddEffect()
        {
            Taker = detector.GetMainTarget();
            Invoker.Hit(Taker);
        }

        private async UniTaskVoid CompleteOnNextFrame()
        {
            cts = new CancellationTokenSource();
            
            await UniTask.Yield(PlayerLoopTiming.Update, cts.Token);
            
            Invoker.Complete();
        }

        private void StopNextFramer()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
