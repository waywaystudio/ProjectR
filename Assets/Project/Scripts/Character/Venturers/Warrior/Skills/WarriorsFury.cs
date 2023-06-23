using System.Threading;
using Common;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Warrior.Skills
{
    public class WarriorsFury : SkillComponent
    {
        [SerializeField] private float additionalHaste = 0.4f;
        
        private CancellationTokenSource cts;
        
        public override void Initialize()
        {
            base.Initialize();

            SequenceBuilder.AddCondition("FullResource", FullResource)
                           .Add(SectionType.Active, "ActiveBuff", AddEffect)
                           .Add(SectionType.Active, "ConsumeResource", () => ConsumeResource().Forget());
        }


        private bool FullResource()
        {
            return Cb.DynamicStatEntry.Resource.Value >= 100f;
        }
        
        private void AddEffect()
        {
            // 가속++
            Cb.StatTable.Add(new StatEntity(StatType.Haste, "WarriorsFury", additionalHaste));
            
            // 몇몇 스킬 변화?
            

            // 기절, 넉백 면역
            Cb.KnockBackBehaviour.Builder.AddCondition("ImmuneByWarriorsFury", () => false);
            Cb.StunBehaviour.Builder.AddCondition("ImmuneByWarriorsFury", () => false);
        }

        private void RemoveEffect()
        {
            // 가속--
            Cb.StatTable.Remove(StatType.Haste, "WarriorsFury");
            
            // 변화된 스킬 원상복구
            
            // 기절, 넉백 면역 해제
            Cb.KnockBackBehaviour.Builder.RemoveCondition("ImmuneByWarriorsFury");
            Cb.StunBehaviour.Builder.RemoveCondition("ImmuneByWarriorsFury");
        }

        private async UniTaskVoid ConsumeResource()
        {
            cts = new CancellationTokenSource();

            while (true)
            {
                Cb.DynamicStatEntry.Resource.Value -= Time.deltaTime * 10f;

                if (Cb.DynamicStatEntry.Resource.Value <= 0)
                {
                    RemoveEffect();
                    StopTask();
                    return;
                }

                await UniTask.Yield(cts.Token);
            }
        }
        
        private void StopTask() => cts?.Cancel();
    }
}
