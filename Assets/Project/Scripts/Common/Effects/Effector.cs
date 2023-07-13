using System;
using System.Collections.Generic;
using Common.Effects.Cameras;
using Common.Effects.HitPauses;
using Common.Effects.Impulses;
using Common.Effects.Particles;
using Common.Effects.PostProcesses;
using Common.Effects.Sounds;
using Common.Effects.Times;
using UnityEngine;

namespace Common.Effects
{
    [Serializable]
    public class Effector
    {
        [SerializeField] private List<CombatParticle> combatParticles;
        [SerializeField] private List<CombatSound> combatSounds;
        [SerializeField] private List<CombatImpulse> combatImpulses;
        [SerializeField] private List<CombatCamera> combatCameras;
        [SerializeField] private List<CombatPostProcess> combatPostProcesses;
        [SerializeField] private List<HitPause> hitPauses;
        [SerializeField] private List<BulletTime> bulletTimes;


        public void Initialize(CombatSequence sequence, IHasTaker takerHolder)
        {
            combatParticles?.ForEach(cp => cp.Initialize(sequence, takerHolder));
            combatSounds?.ForEach(cs => cs.Initialize(sequence));
            combatImpulses?.ForEach(ci => ci.Initialize(sequence));
            combatCameras?.ForEach(cc => cc.Initialize(sequence));
            combatPostProcesses?.ForEach(pp => pp.Initialize(sequence));
            hitPauses?.ForEach(hp => hp.Initialize(sequence));
            bulletTimes?.ForEach(bt => bt.Initialize(sequence));
        }

        public void ActiveEffect(bool activity)
        {
            combatParticles?.ForEach(cp => cp.Activity     = activity);
            combatSounds?.ForEach(cs => cs.Activity        = activity);
            combatImpulses?.ForEach(ci => ci.Activity      = activity);
            combatCameras?.ForEach(cc => cc.Activity       = activity);
            combatPostProcesses?.ForEach(pp => pp.Activity = activity);
            hitPauses?.ForEach(hp => hp.Activity           = activity);
            bulletTimes?.ForEach(bt => bt.Activity         = activity);
        }


#if UNITY_EDITOR
        // for Odin drawer
        private bool isEmptyCombatParticle => combatParticles.IsNullOrEmpty();
        private bool isEmptyCombatSounds => combatSounds.IsNullOrEmpty();
        private bool isEmptyCombatImpulses => combatImpulses.IsNullOrEmpty();
        private bool isEmptyCombatCameras => combatCameras.IsNullOrEmpty();
        private bool isEmptyCombatPostProcesses => combatPostProcesses.IsNullOrEmpty();
        private bool isEmptyHitPauses => hitPauses.IsNullOrEmpty();
        private bool isEmptyBulletTimes => bulletTimes.IsNullOrEmpty();
        
        public void GetEffectsInEditor(Transform transform)
        {
            transform.GetComponentsInChildren(combatParticles);
            transform.GetComponentsInChildren(combatSounds);
            transform.GetComponentsInChildren(combatImpulses);
            transform.GetComponentsInChildren(combatCameras);
            transform.GetComponentsInChildren(combatPostProcesses);
            transform.GetComponentsInChildren(hitPauses);
            transform.GetComponentsInChildren(bulletTimes);
        }
#endif
    }
}
