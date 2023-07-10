using Cameras;
using Cinemachine;
using Singleton;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    private Camera mainCamera;
    private CinemachineBrain brain;
    private CinemachineVirtualCamera activeVCam;

    public static Camera MainCamera => Instance.mainCamera ??= Camera.main;
    public static CinemachineBrain Brain => Instance.brain 
        ? Instance.brain 
        : Instance.brain = MainCamera.GetComponent<CinemachineBrain>();
    public static CinemachineVirtualCamera ActiveCamera => Instance.activeVCam 
        ? Instance.activeVCam 
        : Instance.activeVCam = Brain.ActiveVirtualCamera as CinemachineVirtualCamera;

    public static CameraDirector Director { get; set; }
    public static VirtualCameraType ActiveCameraType => Director.CurrentCameraType;

    
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetSingleton()
    {
        if (!Instance.IsNullOrDestroyed())
            Instance.SetInstanceNull();
    }
}
