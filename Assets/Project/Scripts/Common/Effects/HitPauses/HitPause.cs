using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Effects.HitPauses
{
    public class HitPause : MonoBehaviour
    {
        [SerializeField] private string actionKey = "PlayHitPause";
        [SerializeField] private int millisecond;
        [SerializeField] private Section playSection;
        
        private CancellationTokenSource cts;
        private bool activity = true;
        
        public bool Activity
        {
            get => activity;
            set
            {
                if (value == false)
                {
                    ResumeHitPause();
                }

                activity = value;
            }
        }
        
        public void Initialize(CombatSequence sequence)
        {
            var builder = new CombatSequenceBuilder(sequence);

            if (playSection != Section.None)
            {
                switch (playSection)
                {
                    case Section.Hit:
                    {
                        builder.AddHit(actionKey, _ => PlayHitPause());
                        break;
                    }
                    case Section.SubHit:
                    {
                        builder.AddSubHit(actionKey, _ => PlayHitPause());
                        break;
                    }
                    case Section.Fire:
                    {
                        builder.AddFire(actionKey, _ => PlayHitPause());
                        break;
                    }
                    case Section.SubFire:
                    {
                        builder.AddSubFire(actionKey, _ => PlayHitPause());
                        break;
                    }
                    default:
                    {
                        builder.Add(playSection, actionKey, PlayHitPause);
                        break;
                    }
                }
            }

            builder.Add(Section.End, actionKey, ResumeHitPause);
        }

        public void PlayHitPause()
        {
            if (!Activity) return;
            
            cts?.Cancel();
            cts = new CancellationTokenSource();
            
            HitPauseAsync(cts.Token).Forget();
        }

        public void ResumeHitPause()
        {
            cts?.Cancel();
            cts = null;
            
            Time.timeScale = 1f;
        }


        private async UniTask HitPauseAsync(CancellationToken token)
        {
            Time.timeScale = 0f;

            await UniTask.Delay(millisecond, true, PlayerLoopTiming.Update, token);

            Time.timeScale = 1f;
        }

        private void OnDestroy()
        {
            ResumeHitPause();
        }
    }
}
