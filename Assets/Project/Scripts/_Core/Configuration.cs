using Serialization;
using UnityEngine;

public class Configuration : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;

    private void Awake()
    {
        // Application.targetFrameRate = 60;
        saveManager.LoadAllSaveFile();
    }
}
