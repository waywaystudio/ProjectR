using UnityEngine;

namespace Common.Particles
{
    public class ParticleInstance : MonoBehaviour, IEditable
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool flipEnable;

        private ParticleSystemRenderer particleSystemRenderer;
        private ActionTable OnParticleStopped { get; } = new();

        public ParticleSystemRenderer ParticleSystemRenderer => particleSystemRenderer ??= GetComponent<ParticleSystemRenderer>();
        public Pool<ParticleInstance> Pool { get; set; }
        

        public void Play()
        {
            if (flipEnable) Flip();
            
            particle.Play();
        }

        public void Play(Transform parent) => Play(transform.position + offset, parent);
        public void Play(Vector3 position, Transform parent)
        {
            if (transform.parent != parent)
                transform.SetParent(parent);
            
            transform.position = position + offset;

            if (flipEnable) Flip();
        
            particle.Play(true);
        }

        public void Stop()
        {
            OnParticleStopped.Invoke();
            particle.Stop(true);
        }

        /*
         * ParticleSystem's Native Callback Method
         */
        public void OnParticleSystemStopped()
        {
            Pool?.Release(this);
            OnParticleStopped.Invoke();
        }


        private void Flip()
        {
            ParticleSystemRenderer.flip = transform.forward.x < 0
                ? Vector3.right 
                : Vector3.zero; 
        }

        private void Reset()
        {
            particle = GetComponent<ParticleSystem>();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            particle = GetComponent<ParticleSystem>();
        }
#endif
    }
}