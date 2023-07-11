using System;
using System.Collections.Generic;
using Common.Effects.Cameras;
using Common.Effects.Impulses;
using Common.Effects.Particles;
using Common.Effects.Sounds;
using Common.Effects.Times;
using Common.Projectors;
using UnityEngine;

namespace Common.Effects
{
    [Serializable]
    public class Effector
    {
        [SerializeField] private List<CombatParticle> combatParticles;
        [SerializeField] private List<CombatAudio> combatSounds;
        [SerializeField] private List<CombatImpulse> combatImpulses;
        [SerializeField] private List<CombatCamera> combatCameras;
        [SerializeField] private List<BulletTime> bulletTimes;


        public void Initialize(CombatSequence sequence, IHasTaker takerHolder)
        {
            combatParticles?.ForEach(cp => cp.Initialize(sequence, takerHolder));
            combatSounds?.ForEach(cs => cs.Initialize(sequence));
            combatImpulses?.ForEach(ci => ci.Initialize(sequence));
            combatCameras?.ForEach(cc => cc.Initialize(sequence));
            bulletTimes?.ForEach(bt => bt.Initialize(sequence));
        }

        public void ActiveEffect(bool activity)
        {
            combatParticles?.ForEach(cp => cp.Activity = activity);
            combatSounds?.ForEach(cs => cs.Activity    = activity);
            combatImpulses?.ForEach(ci => ci.Activity  = activity);
            combatCameras?.ForEach(cc => cc.Activity   = activity);
            bulletTimes?.ForEach(bt => bt.Activity     = activity);
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
            transform.GetComponentsInChildren(combatCameras);
            transform.GetComponentsInChildren(bulletTimes);
        }
#endif
    }
}
