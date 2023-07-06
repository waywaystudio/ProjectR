using UnityEngine;

namespace Common.Particles
{
    public class ParticleComponent : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;

        public Sequencer Sequencer { get; } = new();
        public SequenceInvoker Invoker { get; private set; }

        public void Play(Vector3 position, Transform parent = null)
        {
            
        }

        public void Stop()
        {
            
        }

        /* ParticleSystem Native Callback Action Rule */
        public void OnParticleSystemStopped()
        {
            
        }
    }
}
