using UnityEngine;

namespace Common.Effects.Sounds
{
    public class CombatAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private string actionKey = "PlayCombatSound";
        [SerializeField] private bool useRandomPitch;
        [SerializeField] private float randomPitchPercent = 0.1f;
        [SerializeField] private Section playSection;
        [SerializeField] private Section stopSection;
        

        public void Initialize(CombatSequence sequence)
        {
            var builder = new CombatSequenceBuilder(sequence);

            if (playSection != Section.None)
            {
                switch (playSection)
                {
                    case Section.Hit:
                    {
                        builder.AddHit(actionKey, _ => PlayAudio());
                        break;
                    }
                    case Section.SubHit:
                    {
                        builder.AddSubHit(actionKey, _ => PlayAudio());
                        break;
                    }
                    case Section.Fire:
                    {
                        builder.AddFire(actionKey, _ => PlayAudio());
                        break;
                    }
                    case Section.SubFire:
                    {
                        builder.AddSubFire(actionKey, _ => PlayAudio());
                        break;
                    }
                    default:
                    {
                        builder.Add(playSection, actionKey, PlayAudio);
                        break;
                    }
                }
            }

            if (stopSection != Section.None)
            {
                builder.Add(stopSection, actionKey, StopAudio);
            }
        }

        public void PlayAudio()
        {
            if (useRandomPitch)
            {
                source.pitch = 1 + Random.Range(-randomPitchPercent, randomPitchPercent);
            }
            
            source.Play();
        }

        public void StopAudio()
        {
            source.Stop();
        }
    }
}

/* Annotation
 * Random Pitch 외에 Plugin성격으로 Audio Effect에 추가될 컨텐츠가 있다면, 클래스를 분리하자. */
