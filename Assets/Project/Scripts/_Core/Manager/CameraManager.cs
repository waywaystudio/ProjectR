using Cameras;
using Cinemachine;
using Singleton;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    private Camera mainCamera;
    private CinemachineBrain brain;

    public static Camera MainCamera => Instance.mainCamera;
    public static CinemachineBrain Brain => Instance.brain;
    public static CinemachineVirtualCamera ActiveCamera => Brain.ActiveVirtualCamera as CinemachineVirtualCamera;

    public static CameraDirector Director { get; set; }
    public static VirtualCameraType ActiveCameraType => Director.CurrentCameraType;


    public static void SetPlayerToEffectorBlend(CinemachineBlendDefinition definition)
    {
        var blenderSettings = Brain.m_CustomBlends;
        
        for (var i = 0; i < blenderSettings.m_CustomBlends.Length; i++)
        {
            if (blenderSettings.m_CustomBlends[i].m_From != "Player" ||
                blenderSettings.m_CustomBlends[i].m_To   != "Effector")
                continue;
            
            blenderSettings.m_CustomBlends[i].m_Blend = definition;
            return;
        }

        Debug.Log($"Can't Find CustomBlend Between Player and Effector");
    }

    protected override void Awake()
    {
        base.Awake();
        
        mainCamera = Camera.main;
        brain      = MainCamera.GetComponent<CinemachineBrain>();
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetSingleton()
    {
        if (!Instance.IsNullOrDestroyed())
            Instance.SetInstanceNull();
    }
}