using Character.Venturers.Knight.Skills;
using UnityEngine;

namespace Character.Venturers.Knight.Fx
{
    public class BashFx : MonoBehaviour, IEditable
    {
        [SerializeField] private Bash bash;
        [SerializeField] private GameObject BashBonusParticlePrefab;
    
        private ParticleSystem bashBonusParticle;
    
    
        private void PlayBashBonusParticle()
        {
            bashBonusParticle.gameObject.SetActive(true);
            bashBonusParticle.Play(true);
        }

        private void Awake()
        {
            if (!BashBonusParticlePrefab.ValidInstantiate(out bashBonusParticle, transform)) return;

            var builder = new SequenceBuilder(bash.Sequencer);

            builder
                .Add(Section.Extra, "PlaySlashParticle", PlayBashBonusParticle);
            // .Add(SectionType.End, "StopSlashParticle", StopBashBonusParticle);
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            bash = GetComponentInParent<Bash>();
        }
#endif
    }
}
