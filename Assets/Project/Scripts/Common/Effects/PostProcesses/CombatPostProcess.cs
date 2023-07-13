using System;
using UnityEngine;

namespace Common.Effects.PostProcesses
{
    public class CombatPostProcess : MonoBehaviour
    {
        [SerializeField] private string actionKey = "PlayPostProcess";
        [SerializeField] private float intensity = 1.5f;
        [SerializeField] private float duration = 1.5f;
        [SerializeField] private PpType ppType;
        [SerializeField] private Section playSection;
        [SerializeField] private Section stopSection;

        private enum PpType { None = 0, Vignette, Bloom, }

        private bool activity = true;
        public bool Activity
        {
            get => activity;
            set
            {
                if (value == false)
                {
                    StopPostProcess();
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
                        builder.AddHit(actionKey, _ => PlayPostProcess());
                        break;
                    }
                    case Section.SubHit:
                    {
                        builder.AddSubHit(actionKey, _ => PlayPostProcess());
                        break;
                    }
                    case Section.Fire:
                    {
                        builder.AddFire(actionKey, _ => PlayPostProcess());
                        break;
                    }
                    case Section.SubFire:
                    {
                        builder.AddSubFire(actionKey, _ => PlayPostProcess());
                        break;
                    }
                    default:
                    {
                        builder.Add(playSection, actionKey, PlayPostProcess);
                        break;
                    }
                }
            }

            if (stopSection != Section.None)
            {
                builder.Add(stopSection, actionKey, StopPostProcess);
            }

            builder.Add(Section.End, actionKey, StopPostProcess);
        }

        public void PlayPostProcess()
        {
            if (!Activity) return;
            
            switch (ppType) 
            {
                case PpType.Vignette:
                {
                    PostProcessingManager.Vignetting(intensity, duration);
                    break;
                }
                case PpType.Bloom:
                {
                    PostProcessingManager.Blooming(intensity, duration);
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void StopPostProcess()
        {
            switch (ppType) 
            {
                case PpType.Vignette:
                {
                    PostProcessingManager.ResetVignetting();
                    break;
                }
                case PpType.Bloom:
                {
                    PostProcessingManager.ResetBlooming();
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
