using Core;
using Main;
using UnityEngine;
using UnityEngine.Events;
using Debug = Debug;

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

    // TODO. temp event
    public void ToRaidScene()
    {
        global::Debug.Log("ObstacleEvent.MainEvent Invoked!");
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
