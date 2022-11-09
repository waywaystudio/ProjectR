using Main;
using Main.Input;
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

    // TODO. temp event
    public void MainEventTestFunction() => Debug.Log("ObstacleEvent.MainEvent Invoked!");

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
