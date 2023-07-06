using Character.Venturers.Knight.Skills;
using UnityEngine;

namespace Character.Venturers.Knight.Fx
{
    public class SlashFx : MonoBehaviour, IEditable
    {
        [SerializeField] private Slash slash;
        [SerializeField] private GameObject slashParticlePrefab;
        
        private ParticleSystem slashParticle;
        private ParticleSystemRenderer slashParticleRenderer;

        private void PlaySlashParticle()
        {
            slashParticleRenderer.flip = slash.Provider.Forward.x < 0 ? Vector3.right : Vector3.zero;
            
            slashParticle.gameObject.SetActive(true);
            slashParticle.Play(true);
        }

        private void Awake()
        {
            if (!slashParticlePrefab.ValidInstantiate(out slashParticle, transform)) return;
            if (!slashParticle.TryGetComponent(out slashParticleRenderer)) return;

            var builder = new SequenceBuilder(slash.Sequencer);

            builder
                .Add(SectionType.Execute, "PlaySlashParticle", PlaySlashParticle);
            // .Add(SectionType.End, "StopSlashParticle", StopSlashParticle);
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            slash = GetComponentInParent<Slash>();
        }
#endif
    }
}
