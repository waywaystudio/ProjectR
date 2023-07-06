using UnityEngine;

namespace Common.Particles
{
    public class ParticleComponent : MonoBehaviour, IEditable
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private SectionType playSection;
        [SerializeField] private SectionType stopSection;

        public SinglePool<ParticleComponent> Pool { get; set; }
        
        public void Play()
        {
            particle.Play(true);
        }

        public void Play(Vector3 position)
        {
            transform.position = position;
            particle.Play(true);
        }

        public void Stop()
        {
            transform.position = Vector3.zero;
            particle.Stop(true);
        }

        /* ParticleSystem Native Callback Action Rule */
        public void OnParticleSystemStopped()
        {
            transform.position = Vector3.zero;
            Pool.Release();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            particle = GetComponent<ParticleSystem>();
        }
#endif
    }
}
