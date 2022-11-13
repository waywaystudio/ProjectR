using Core;
using Main;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleEvent : MonoBehaviour, IEventModel
{
    [SerializeField] private UnityEvent mainEvent;

    public void Register()
    {
        MainGame.InputManager.Register(this);
    }

    public void Invoke()
    {
        mainEvent?.Invoke();
    }

    public void ToRaidScene()
    {
        MainGame.SceneManager.LoadScene("Raid");
    }

    public void Unregister()
    {
        if (MainGame.Instance is null) return;
        
        MainGame.InputManager.Unregister(this);
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
