using Serialization;
using UnityEngine;

public class Configuration : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;

    private void Awake()
    {
        saveManager.LoadAllSaveFile();
    }
}
