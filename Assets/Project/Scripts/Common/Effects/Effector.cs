using System;
using System.Collections.Generic;
using Common.Effects.Impulse;
using Common.Effects.Particles;
using Common.Effects.Sounds;
using UnityEngine;

namespace Common.Effects
{
    [Serializable]
    public class Effector
    {
        [SerializeField] private List<CombatParticle> combatParticles;
        [SerializeField] private List<CombatAudio> combatSounds;
        [SerializeField] private List<CombatImpulse> combatImpulses;


        public void Initialize(CombatSequence sequence, IHasTaker takerHolder)
        {
            combatParticles?.ForEach(cp => cp.Initialize(sequence, takerHolder));
            combatSounds?.ForEach(cs => cs.Initialize(sequence));
            combatImpulses?.ForEach(ci => ci.Initialize(sequence));
        }

        public void Dispose()
        {
            combatParticles?.ForEach(cp => cp.Dispose());
        }


#if UNITY_EDITOR
        public void GetEffectsInEditor(Transform transform)
        {
            transform.GetComponentsInChildren(combatParticles);
            transform.GetComponentsInChildren(combatSounds);
            transform.GetComponentsInChildren(combatImpulses);
        }
#endif
    }
}
