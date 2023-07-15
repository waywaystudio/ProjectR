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


        public void Initialize(ICombatObject combatObject)
        {
            combatParticles?.ForEach(cp => cp.Initialize(combatObject));
            combatSounds?.ForEach(cs => cs.Initialize(combatObject.Sequence));
            combatImpulses?.ForEach(ci => ci.Initialize(combatObject.Sequence));
            combatCameras?.ForEach(cc => cc.Initialize(combatObject.Sequence));
            combatPostProcesses?.ForEach(pp => pp.Initialize(combatObject.Sequence));
            hitPauses?.ForEach(hp => hp.Initialize(combatObject.Sequence));
            bulletTimes?.ForEach(bt => bt.Initialize(combatObject.Sequence));
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
