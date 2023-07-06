using Character.Venturers.Knight.Skills;
using UnityEngine;

namespace Character.Venturers.Knight.Fx
{
    public class ShieldUpFx : MonoBehaviour, IEditable
    {
        [SerializeField] private ShieldUp shieldUp;
        [SerializeField] private GameObject shieldUpPrefab;
        
        private ParticleSystem shieldUpParticle;
        
        
        private void PlayShieldUpParticle()
        {
            // slashParticleRenderer.flip = slash.Provider.Forward.x < 0 ? Vector3.right : Vector3.zero;
            
            shieldUpParticle.gameObject.SetActive(true);
            shieldUpParticle.Play(true);
        }
        
        private void StopShieldUpParticle()
        {
            shieldUpParticle.gameObject.SetActive(false);
            shieldUpParticle.Stop(true);
        }
        
        private void Awake()
        {
            if (!shieldUpPrefab.ValidInstantiate(out shieldUpParticle, transform)) return;

            // slashParticleRenderer = slashParticle.GetComponent<ParticleSystemRenderer>();

            var builder = new SequenceBuilder(shieldUp.Sequencer);
        
            builder
                .Add(Section.Active, "PlaySlashParticle", PlayShieldUpParticle)
                .Add(Section.End, "StopSlashParticle", StopShieldUpParticle);
        }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            shieldUp = GetComponentInParent<ShieldUp>();
        }
#endif
    }
}
