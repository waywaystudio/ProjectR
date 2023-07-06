using System;
using UnityEngine;

namespace Common.Particles
{
    [Serializable]
    public class ParticlePool : SinglePool<ParticleComponent>
    {
        public void Play() => Get().Play();
        public void Play(Vector3 position) => Get().Play(position);
        public void Stop() => Release();
    }
}
