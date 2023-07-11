using System.Threading;
using Common;
using Common.Characters;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Warrior.Skills
{
    public class WarriorsFury : SkillComponent
    {
        [SerializeField] private float additionalHaste = 0.4f;
        [SerializeField] private float resourceConsumePerSecond = 10f;
        
        private CancellationTokenSource cts;
        
        public override void Initialize()
        {
            base.Initialize();

            Builder.AddCondition("FullResource", FullResource)
                           .Add(Section.Active, "ActiveBuff", AddEffect)
                           .Add(Section.Active, "ConsumeResource", () => ConsumeResource().Forget());
        }


        private bool FullResource()
        {
            return Cb.Resource.Value >= 100f;
        }
        
        private void AddEffect()
        {
            // 가속++
            Cb.StatTable.Add(new StatEntity(StatType.Haste, "WarriorsFury", additionalHaste));
            
            // 몇몇 스킬 변화?
            var sb = Cb.SkillTable;
            
            sb.ChangeSkill(DataIndex.Smash, DataIndex.BloodSmash);
            sb.ChangeSkill(DataIndex.LeapAttack, DataIndex.RapidCharge);
            sb.ChangeSkill(DataIndex.Deathblow, DataIndex.Exploitation);
            sb.ChangeSkill(DataIndex.WarriorsFury, DataIndex.Finisher);
            

            // 기절, 넉백 면역
            Cb.KnockBackBehaviour.Builder.AddCondition("ImmuneByWarriorsFury", () => false);
            Cb.StunBehaviour.Builder.AddCondition("ImmuneByWarriorsFury", () => false);
        }

        private void RemoveEffect()
        {
            // 가속--
            Cb.StatTable.Remove(StatType.Haste, "WarriorsFury");
            
            // 변화된 스킬 원상복구
            var sb = Cb.SkillTable;
            
            sb.ChangeSkill(DataIndex.BloodSmash, DataIndex.Smash);
            sb.ChangeSkill(DataIndex.RapidCharge, DataIndex.LeapAttack);
            sb.ChangeSkill(DataIndex.Exploitation, DataIndex.Deathblow);
            sb.ChangeSkill(DataIndex.Finisher, DataIndex.WarriorsFury);
            
            // 기절, 넉백 면역 해제
            Cb.KnockBackBehaviour.Builder.RemoveCondition("ImmuneByWarriorsFury");
            Cb.StunBehaviour.Builder.RemoveCondition("ImmuneByWarriorsFury");
        }

        private async UniTaskVoid ConsumeResource()
        {
            cts = new CancellationTokenSource();

            while (true)
            {
                Cb.Resource.Value -= Time.deltaTime * resourceConsumePerSecond;

                if (Cb.Resource.Value <= 0)
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
