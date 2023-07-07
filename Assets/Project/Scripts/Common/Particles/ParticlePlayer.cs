using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Common.Particles
{
    [Serializable]
    public class ParticlePlayer
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool isLoop;
        [SerializeField] private Section playSection;
        [SerializeField] private Section stopSection;

        private ParticleComponent singleComponent;
        private Transform initialParent;
        private bool isInitialized;
        private IObjectPool<ParticleComponent> objectPool;
        
        public IActionSender ActionSender { get; set; }
        public Section PlaySection => playSection;
        public Section StopSection => stopSection;
        public ParticleSystem ParticleSystem => singleComponent.ParticleSystem;
        public ParticleSystemRenderer ParticleRenderer => singleComponent.ParticleSystemRenderer;
        
        
        public void Initialize(IActionSender actionSender, Transform initialParent = null)
        {
            objectPool?.Clear();
            objectPool = new ObjectPool<ParticleComponent>(
                () => Create(initialParent), 
                component => component.gameObject.SetActive(true), 
                component => component.gameObject.SetActive(false), 
                component => Object.Destroy(component.gameObject),
                true, 
                0, 
                1);

            this.initialParent = initialParent;
            ActionSender       = actionSender;
            isInitialized      = true;
        }

        public ParticleComponent Get() => objectPool.Get();

        public void Play() => Play(initialParent.position, initialParent);
        public void Play(Vector3 position) => Play(position, initialParent);
        public void Play(Transform parent) => Play(initialParent.position, parent);
        public void Play(Vector3 position, Transform parent)
        {
            if (!isInitialized)
            {
                Debug.LogError("PoolingSystem not Initialized. please run Initialize Function before use PoolingSystem. "
                               + $"Input:{prefab.name}");
                return;
            }

            var pc = objectPool.Get();
            var particleTransform = pc.transform;
            
            particleTransform.SetParent(parent);
            pc.Play(position + particleTransform.localPosition, parent);
        }

        public void Stop()
        {
            if (isInitialized)
            {
                // Debug.Log($"Stopped. PoolID:{objectPool.GetHashCode()} Pool Count:{objectPool.CountInactive}");
                objectPool.Release(singleComponent);
                return;
            }
            
            Debug.LogError("PoolingSystem not Initialized. please run Initialize Function before use PoolingSystem. "
                           + $"Input:{prefab.name}");
        }

        public void Clear() => objectPool.Clear();
        
        
        private ParticleComponent Create(Transform parent)
        {
            if (!prefab.ValidInstantiate(out singleComponent, parent)) return null;
        
            singleComponent.gameObject.SetActive(false);
            singleComponent.AddStopAction("ReleasePool", Stop);

            return singleComponent;
        }
    }
}
