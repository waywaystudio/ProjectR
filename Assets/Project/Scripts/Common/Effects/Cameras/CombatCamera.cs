using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Common.Effects.Cameras
{
    public class CombatCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineBlendDefinition blend;
        [SerializeField] private float magnification = 0.8f;
        [SerializeField] private string actionKey = "PlayCameraWalk";
        [SerializeField] private VirtualCameraType targetCamera;
        [SerializeField] private Section playSection;
        [SerializeField] private Section stopSection;

        private Tween cameraTween;
        private float originalFov;
        private bool activity = true;
        
        public bool Activity
        {
            get => activity;
            set
            {
                if (value == false) ReturnZoom();

                activity = value;
            }
        }
        

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
            if (!Activity) return;
            if (targetCamera != CameraManager.ActiveCameraType) return;

            var effectorCamera = CameraManager.Director[VirtualCameraType.Effector];
            var playerCameraFov = CameraManager.Director[VirtualCameraType.Player].m_Lens.FieldOfView;
            
            effectorCamera.m_Lens.FieldOfView = playerCameraFov * magnification;
            
            CameraManager.SetPlayerToEffectorBlend(blend);
            CameraManager.Director.ChangeCamera(VirtualCameraType.Effector);
        }

        public void ReturnZoom()
        {
            if (!Activity) return;
            
            CameraManager.Director.ChangeCamera(VirtualCameraType.Player);
        }
    }
}
