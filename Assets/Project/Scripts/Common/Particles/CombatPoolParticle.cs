using System;
using UnityEngine;

namespace Common.Particles
{
    [Serializable]
    public class CombatPoolParticle : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private Pool<ParticleComponent> particlePool;
        [SerializeField] private ParticleSpawnParent parent;
        [SerializeField] private PrepositionType prepositionType;
        [SerializeField] private Section playSection;
        
        private ISequencerHolder sequenceHolder;
        private IHasTaker takerHolder;
        private Transform Parent
        {
            get
            {
                var taker = takerHolder.Taker;
            
                if (takerHolder?.Taker is null) 
                    return null;
                
                return parent switch
                {
                    ParticleSpawnParent.Self  => transform,
                    ParticleSpawnParent.Taker => taker.gameObject.transform,
                    ParticleSpawnParent.World => null,
                    _                         => null
                };
            }
        }

        private Vector3 SpawnPosition
        {
            get
            {
                var taker = takerHolder.Taker;
            
                if (takerHolder?.Taker is null) 
                    return transform.position;

                return prepositionType switch
                {
                    PrepositionType.None   => transform.position,
                    PrepositionType.Top    => taker.Position,
                    PrepositionType.Head   => taker.Position,
                    PrepositionType.Body   => taker.Position + Vector3.up * 3f,
                    PrepositionType.Origin => taker.Position,
                    _                      => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private void Awake()
        {
            sequenceHolder = GetComponentInParent<ISequencerHolder>();
            takerHolder    = GetComponentInParent<IHasTaker>(); 

            var builder = new SequenceBuilder(sequenceHolder.Sequencer);

            if (playSection != Section.None) builder.Add(playSection, key, () => particlePool.Get().Play(SpawnPosition, Parent));

            particlePool.Initialize(component => component.Pool = particlePool);
        }

        private void OnDestroy()
        {
            particlePool?.Clear();
        }
    }
}
