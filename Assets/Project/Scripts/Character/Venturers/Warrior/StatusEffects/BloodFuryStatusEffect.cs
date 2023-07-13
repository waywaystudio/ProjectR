using System.Threading;
using Common;
using Common.StatusEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Venturers.Warrior.StatusEffects
{
    public class BloodFuryStatusEffect : StatusEffect
    {
        [SerializeField] private float additionalHaste = 0.4f;
        [SerializeField] private float resourceConsumePerSecond = 10f;
        
        private CancellationTokenSource cts;
        
        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            Builder
                .Add(Section.Active,"StatChangeExecution", StatChangeExecution)
                .Add(Section.Active, "ConsumeResource", () => ConsumeResource().Forget())
                .Add(Section.End, "StopTask", StopTask)
                ;
        }

        protected override void Dispose()
        {
            base.Dispose();

            StopTask();
        }


        private void StatChangeExecution()
        {
            // 가속++
            Taker.StatTable.Add(new StatEntity(StatType.Haste, "WarriorsFury", additionalHaste));
            
            // 기절, 넉백 면역
            Taker.KnockBackBehaviour.Builder.AddCondition("ImmuneByWarriorsFury", () => false);
            Taker.StunBehaviour.Builder.AddCondition("ImmuneByWarriorsFury", () => false);

            var sb = Provider.SkillTable;
            
            sb.ChangeSkill(DataIndex.Smash, DataIndex.BloodSmash);
            sb.ChangeSkill(DataIndex.LeapAttack, DataIndex.RapidCharge);
            sb.ChangeSkill(DataIndex.Deathblow, DataIndex.Exploitation);
            sb.ChangeSkill(DataIndex.WarriorsFury, DataIndex.Finisher);
        }

        private void ReturnToOrigin()
        {
            // 가속--
            Taker.StatTable.Remove(StatType.Haste, "WarriorsFury");
            
            // 기절, 넉백 면역 해제
            Taker.KnockBackBehaviour.Builder.RemoveCondition("ImmuneByWarriorsFury");
            Taker.StunBehaviour.Builder.RemoveCondition("ImmuneByWarriorsFury");
            
            // 변화된 스킬 원상복구
            var sb = Provider.SkillTable;
            
            sb.ChangeSkill(DataIndex.BloodSmash, DataIndex.Smash);
            sb.ChangeSkill(DataIndex.RapidCharge, DataIndex.LeapAttack);
            sb.ChangeSkill(DataIndex.Exploitation, DataIndex.Deathblow);
            sb.ChangeSkill(DataIndex.Finisher, DataIndex.WarriorsFury);
        }
        
        private async UniTaskVoid ConsumeResource()
        {
            cts = new CancellationTokenSource();

            while (true)
            {
                Taker.Resource.Value -= Time.deltaTime * resourceConsumePerSecond;

                if (Taker.Resource.Value <= 0)
                {
                    ReturnToOrigin();
                    break;
                }

                await UniTask.Yield(cts.Token);
            }
            
            Invoker.Complete();
        }
        
        private void StopTask()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
