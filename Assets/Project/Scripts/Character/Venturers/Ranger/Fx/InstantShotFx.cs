using System;
using Character.Venturers.Ranger.Skills;
using Common;
using UnityEngine;

namespace Character.Venturers.Ranger.Fx
{
    public class InstantShotFx : MonoBehaviour, IEditable
    {
        [SerializeField] private InstantShot instantShot;
        
        // Vfx
        [SerializeField] private Pool<ParticleSystem> pool;
        
        // Sfx
        
        // Cfx

        private void FireInstantShotParticle()
        {
            
        }

        private void CreateArrow(ParticleSystem particleSystem)
        {
            instantShot.Builder
                       .Add(Section.Active, "PlayArrowParticle", particleSystem.Play)
                       .Add(Section.End, "StopArrowParticle", particleSystem.Stop);
        }

        private void OnEnable()
        {
            pool.Initialize(CreateArrow, transform);
        }

        private void OnDisable()
        {
            pool.Clear();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            instantShot = GetComponentInParent<InstantShot>();

            if (instantShot.IsNullOrDestroyed())
            {
                Debug.LogWarning("Not Exist InstantShot in Parent");
            }
        }
#endif
    }
}
