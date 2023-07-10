using DG.Tweening;
using UnityEngine;

namespace Common.Effects.Cameras
{
    public class CombatCamera : MonoBehaviour
    {
        [SerializeField] private VirtualCameraType targetCamera;
        [SerializeField] private float magnification = 1f;
        [SerializeField] private float duration;
        [SerializeField] private string actionKey = "PlayCameraWalk";
        [SerializeField] private Ease ease;
        [SerializeField] private bool unscaledTime;
        [SerializeField] private Section playSection;
        [SerializeField] private Section stopSection;

        private float originalFov;
        private Tween cameraTween;

        public void Initialize(CombatSequence sequence)
        {
            var builder = new CombatSequenceBuilder(sequence);

            if (playSection != Section.None)
            {
                switch (playSection)
                {
                    case Section.Hit:
                    {
                        builder.AddHit(actionKey, _ => PlayZoom());
                        break;
                    }
                    case Section.SubHit:
                    {
                        builder.AddSubHit(actionKey, _ => PlayZoom());
                        break;
                    }
                    case Section.Fire:
                    {
                        builder.AddFire(actionKey, _ => PlayZoom());
                        break;
                    }
                    case Section.SubFire:
                    {
                        builder.AddSubFire(actionKey, _ => PlayZoom());
                        break;
                    }
                    default:
                    {
                        builder.Add(playSection, actionKey, PlayZoom);
                        break;
                    }
                }
            }
            
            if (stopSection != Section.None)
            {
                builder.Add(stopSection, actionKey, ReturnZoom);
            }

            builder.Add(Section.End, actionKey, ReturnZoom);
        }
        

        public void PlayZoom()
        {
            if (targetCamera != CameraManager.ActiveCameraType) return;
            
            // Get the current active virtual camera
            var activeVCam = CameraManager.ActiveCamera;

            if (activeVCam != null)
            {
                // Store the original Field of View (FOV)
                originalFov = activeVCam.m_Lens.FieldOfView;

                // Calculate the target FOV
                var targetFov = originalFov * magnification;

                cameraTween = DOTween.To(() => activeVCam.m_Lens.FieldOfView, 
                                         x => activeVCam.m_Lens.FieldOfView = x, 
                                         targetFov, 
                                         duration)
                                     .SetEase(ease)
                                     // .SetUpdate(unscaledTime)
                                     ;
            }
        }

        public void ReturnZoom()
        {
            if (cameraTween != null)
            {
                cameraTween.Kill();
                cameraTween = null;
            }
            
            var activeVCam = CameraManager.ActiveCamera;
            
            if (activeVCam != null)
            {
                cameraTween = DOTween.To(() => activeVCam.m_Lens.FieldOfView, 
                                         x => activeVCam.m_Lens.FieldOfView = x, 
                                         originalFov, 
                                         0.23f)
                                     // .SetUpdate(unscaledTime)
                                     ;
            }
        }
    }
}
