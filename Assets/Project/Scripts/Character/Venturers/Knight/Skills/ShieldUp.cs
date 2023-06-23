using System.Threading;
using Common;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Knight.Skills
{
    public class ShieldUp : SkillComponent
    {
        private CancellationTokenSource cts;

        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.Add(SectionType.Active, "ActiveBuff", AddEffect)
                           .Add(SectionType.Active, "ConsumeResource", () => ConsumeResource().Forget())
                           .Add(SectionType.End, "RemoveBuff", RemoveEffect)
                           .Add(SectionType.End, "StopTask", StopTask);
        }
        
        
        private void AddEffect()
        {
            // 방어력++
            Cb.StatTable.Add(new StatEntity(StatType.Armor, "WillOfKnight", 100));
            
            // 기절, 넉백 면역
            Cb.KnockBackBehaviour.Builder.AddCondition("ImmuneByWillOfKnight", () => false);
            Cb.StunBehaviour.Builder.AddCondition("ImmuneByWillOfKnight", () => false);
        }

        private void RemoveEffect()
        {
            // 방어력--
            Cb.StatTable.Remove(StatType.Armor, "WillOfKnight");
            
            // 기절, 넉백 면역 해제
            Cb.KnockBackBehaviour.Builder.RemoveCondition("ImmuneByWillOfKnight");
            Cb.StunBehaviour.Builder.RemoveCondition("ImmuneByWillOfKnight");
        }

        private async UniTaskVoid ConsumeResource()
        {
            cts = new CancellationTokenSource();
            
            await UniTask.Delay(500, false, PlayerLoopTiming.Update, cts.Token);

            while (true)
            {
                Cb.DynamicStatEntry.Resource.Value -= Time.deltaTime * 10f;

                if (Cb.DynamicStatEntry.Resource.Value <= 0 || !SkillInvoker.IsActive)
                {
                    Debug.Log("ShieldUp End");
                    Cancel();
                    return;
                }

                await UniTask.Yield(cts.Token);
            }
        }
        
        private void StopTask() => cts?.Cancel();
    }
}