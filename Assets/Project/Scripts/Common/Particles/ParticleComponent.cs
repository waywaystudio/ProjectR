using System;
using UnityEngine;

namespace Common.Particles
{
    public class ParticleComponent : MonoBehaviour, IEditable
    {
        [SerializeField] private ParticleSystem particle;

        public ParticleSystem ParticleSystem => particle;
        public ParticleSystemRenderer ParticleSystemRenderer => particleSystemRenderer ??= GetComponent<ParticleSystemRenderer>();

        private ParticleSystemRenderer particleSystemRenderer;
        private ActionTable OnParticleStopped { get; } = new();

        public Pool<ParticleComponent> Pool { get; set; }

        public void AddStopAction(string key, Action callback)
        {
            OnParticleStopped.Add(key, callback);
        }
        

        public void Play(Transform parent)
        {
            transform.SetParent(parent);
            
            particle.Play(true);
        }
        
        public void Play(Vector3 position, Transform parent)
        {
            transform.position = position;
            transform.SetParent(parent);
            
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


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            particle = GetComponent<ParticleSystem>();
        }
#endif
    }
}
