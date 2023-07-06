using System;
using UnityEngine;

namespace Common.Particles
{
    // 1. Slash ParticleType
        //  주체에 완전히 종속.
        //  ex.Slash, Projectile
    // 2. Impact ParticleType
        //  주체가 생성, 파티클은 독립
        //  주체의 Active 혹은 Execute 시점에 발생하지만, End는 독립적
        //  ex.ProjectileImpact, Trap Explosion
    // 3. StatusEffect ParticleType
        // 주체가 생성, 객체에 종속
        // StatusEffect Component가 따로 있기 때문에, 1번과 같지 않나? 다른예시를 생각해보자.
    
    /// <summary>
    /// Parent 시점. 
    /// </summary>
    // 1의 Home, Work == 주체 Child로 동일
    // 2의 Home, Work == 완전 독립으로 동일
    
    /// <summary>
    /// Play Position 시점
    /// </summary>
    // 1번이든 2번이든 별도의 포지션이 필요한 경우가 있음
    // 경우에 따라서 적당히 offset만 해줘도 되지만, Taker.TransformVariant가 필요하기도 함.
    // 별도의 offset인 경우, ParticleComponent의 Transform.Position Inspector를 수정하면 될 일.
    // Taker의 TransformVariant가 문제.
    
    [Serializable]
    public class ParticleEffect
    {
        // [SerializeField] private bool isSingleSpawnType;
        // [SerializeField] private Pool<ParticleComponent> particlePool;
        // [SerializeField] private Transform storage;
        //
        // private ParticleComponent SingleParticle { get; set; }
        //
        // public void Play(Vector3 position = default)
        // {
        //     var particle = particlePool.Get();
        //
        //     if (isSingleSpawnType)
        //     {
        //         SingleParticle = particle;
        //     }
        //
        //     particle.Play(position);
        // }
        //
        // public void Stop()
        // {
        //     if (!isSingleSpawnType || SingleParticle.IsNullOrDestroyed()) return;
        //
        //     SingleParticle.Stop();
        //     SingleParticle = null;
        // }
        //
        //
        // public void Initialize()
        // {
        //     particlePool.Initialize(CreateHitParticle, storage);
        // }
        //
        // public void Dispose()
        // {
        //     particlePool.Clear();
        // }
        //
        //
        // private void CreateHitParticle(ParticleComponent particle)
        // {
        //     particle.Pool = particlePool;
        // }
    }
}
