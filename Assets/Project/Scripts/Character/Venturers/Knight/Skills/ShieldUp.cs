using System.Threading;
using Common;
using Common.Particles;
using Common.Skills;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Venturers.Knight.Skills
{
    public class ShieldUp : SkillComponent
    {
        [SerializeField] private float armorUpValue = 250f;
        [SerializeField] private SinglePool<ParticleInstance> shieldUpParticle;
        
        private CancellationTokenSource cts;

        public override void Initialize()
        {
            base.Initialize();
            
            shieldUpParticle.Initialize(null, transform);

            Builder
                .Add(Section.Active, "ActiveBuff", AddEffect)
                .Add(Section.Active, "ConsumeResource", () => ConsumeResource().Forget())
                .Add(Section.Active, "PlayShieldUpParticle", PlayShieldUpParticle)
                .Add(Section.End, "RemoveBuff", RemoveEffect)
                .Add(Section.End, "StopTask", StopTask)
                .Add(Section.End, "PlayShieldUpParticle", StopShieldUpParticle);
        }
        
        
        private void AddEffect()
        {
            // 방어력++
            Cb.StatTable.Add(new StatEntity(StatType.Armor, "WillOfKnight", armorUpValue));
            
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
                Cb.Resource.Value -= Time.deltaTime * 10f;

                if (Cb.Resource.Value <= 0 || !Invoker.IsActive)
                {
                    Cancel();
                    return;
                }

                await UniTask.Yield(cts.Token);
            }
        }

        private void StopTask()
        {
            cts?.Cancel();
            cts = null;
        }
        
        private void PlayShieldUpParticle()
        {
            shieldUpParticle.Get().Play();
        }
        
        private void StopShieldUpParticle()
        {
            shieldUpParticle.Release();
        }
    }
}
