using System;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Effects.Impulses
{
    public class CombatImpulse : MonoBehaviour, IEditable
    {
        [SerializeField] private CinemachineImpulseSource source;
        [SerializeField] private string actionKey = "CombatImpulse";
        [SerializeField] private float impulseForce = 1f;
        [SerializeField] private bool isLoop;
        [SerializeField] private Section playSection;
        [SerializeField] private Section endSection;

        private CancellationTokenSource cts;
        private float Duration { get; set; }
        private static Vector3 RandomVelocity => new(
            Random.Range(-1, 2) > 0 ? 1f : -1f, 
            Random.Range(-1, 2) > 0 ? 1f : -1f, 
            Random.Range(-1, 2) > 0 ? 1f : -1f
        );
        
        private bool activity = true;
        public bool Activity
        {
            get => activity;
            set
            {
                if (value == false)
                {
                    StopImpulse();
                }

                activity = value;
            }
        }


        public void Initialize(CombatSequence sequence)
        {
            Duration = source.m_ImpulseDefinition.m_ImpulseDuration;
            
            var builder = new CombatSequenceBuilder(sequence);
            
            if (playSection != Section.None)
            {
                switch (playSection)
                {
                    case Section.Hit:
                    {
                        builder.AddHit(actionKey, _ => PlayImpulse());
                        break;
                    }
                    case Section.SubHit:
                    {
                        builder.AddSubHit(actionKey, _ => PlayImpulse());
                        break;
                    }
                    case Section.Fire:
                    {
                        builder.AddFire(actionKey, _ => PlayImpulse());
                        break;
                    }
                    case Section.SubFire:
                    {
                        builder.AddSubFire(actionKey, _ => PlayImpulse());
                        break;
                    }
                    default:
                    {
                        builder.Add(playSection, actionKey, PlayImpulse);
                        break;
                    }
                }
            }
            
            if (endSection != Section.None) builder.Add(endSection, actionKey, StopImpulse);
            
            builder.Add(Section.End, "StopImpulse", StopImpulse);
        }

        public void PlayImpulse()
        {
            if (!Activity) return;
            if (!isLoop)
            {
                source.GenerateImpulseWithVelocity(impulseForce * RandomVelocity);
            }
            else
            {
                PlayLoop().Forget();
            }
        }

        public void StopImpulse()
        {
            source.GenerateImpulseWithVelocity(Vector3.zero);

            if (isLoop)
            {
                cts?.Cancel();
                cts = null;
            }
        }


        private async UniTaskVoid PlayLoop()
        {
            cts = new CancellationTokenSource();

            while (true)
            {
                source.GenerateImpulseWithVelocity(impulseForce * RandomVelocity);
                
                await UniTask.Delay(TimeSpan.FromSeconds(Duration), DelayType.DeltaTime, PlayerLoopTiming.Update, cts.Token);

                if (cts.IsCancellationRequested) return;
            }
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            source = GetComponent<CinemachineImpulseSource>();
        }
#endif
    }
}
