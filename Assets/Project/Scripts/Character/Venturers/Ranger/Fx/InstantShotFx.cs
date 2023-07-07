using Character.Venturers.Ranger.Skills;
using UnityEngine;

namespace Character.Venturers.Ranger.Fx
{
    public class InstantShotFx : MonoBehaviour, IEditable
    {
        [SerializeField] private InstantShot instantShot;


        private void CreateArrow(ParticleSystem particleSystem)
        {
            instantShot.Builder
                       .Add(Section.Active, "PlayArrowParticle", particleSystem.Play)
                       .Add(Section.End, "StopArrowParticle", particleSystem.Stop);
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
