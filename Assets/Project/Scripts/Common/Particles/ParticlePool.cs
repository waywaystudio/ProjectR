using System;
using UnityEngine;

namespace Common.Particles
{
    [Serializable]
    public class ParticlePool : Pool<ParticleInstance>
    {
        [SerializeField] private bool singleInstanceOnly;

        private ParticleInstance uniqueParticle;

        public void Release() => Release(uniqueParticle);
        
        
        protected override ParticleInstance Create(Action<ParticleInstance> onActivated, Transform parent)
        {
            if (!prefab.ValidInstantiate(out ParticleInstance component, parent)) return null;

            if (singleInstanceOnly)
            {
                uniqueParticle = component;
                uniqueParticle.gameObject.SetActive(false);
            }
            
            onActivated?.Invoke(component);

            return component;
        }
    }
}

/*
 * Annotation
 * singleInstanceOnly는, 단 하나의 오브젝트만 생성하여 재사용하기 위한 변수.
 * 일반적으로 풀링을 사용할 때 하나의 오브젝트만 생성하지 않는다. 그리고 원한다면, MaxCount 를 1로 잡아도 동일한 효과를 가질 수 있다.
 * 그러나 루프타입 파티클을 다룰 때, 파라메타를 받지 않는 Release() 함수의 존재가 꼭 필요하기 때문에 별도의 장치를 마련하였다.
 */
