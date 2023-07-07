using System;
using UnityEngine;

namespace Common.Particles
{
    [Serializable]
    public class CombatSingleParticle : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private SinglePool<ParticleComponent> singleParticle;
        [SerializeField] private ParticleSpawnParent parent;
        [SerializeField] private PrepositionType prepositionType;
        [SerializeField] private Section playSection;
        [SerializeField] private Section endSection;
        
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

            if (playSection != Section.None) builder.Add(playSection, key, () => singleParticle.Get().Play(SpawnPosition, Parent));
            if (endSection  != Section.None) builder.Add(endSection, key, singleParticle.Release);

            singleParticle.Initialize(null, transform);
        }

        private void OnDestroy()
        {
            singleParticle?.Clear();
        }
    }
}
