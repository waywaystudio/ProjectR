using Core;
using MainGame;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent mainEvent;

    public void Register()
    {
        // MainManager.InputManager.Register(this);
    }

    public void Invoke()
    {
        mainEvent?.Invoke();
    }

    public void ToTownScene()
    {
        MainManager.SceneManager.LoadScene("Town");
    }

    public void ToRaidScene()
    {
        MainManager.SceneManager.LoadScene("Raid");
    }

    public void Unregister()
    {
        if (MainManager.Instance is null) return;
        
        // MainManager.InputManager.Unregister(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
            Register();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Unregister();
    }

    private void OnDisable()
    {
        Unregister();
    }
}
