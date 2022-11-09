using Main;
using Main.Input;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleEvent : MonoBehaviour, IEventModel
{
    [SerializeField] private UnityEvent mainEvent;
    
    private SphereCollider trigger;

    private void Awake()
    {
        trigger ??= GetComponent<SphereCollider>();
    }

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
        if (MainGame.InputManager is not null)
            MainGame.InputManager.Unregister(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        Register();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        Unregister();
    }

    private void OnDisable()
    {
        Unregister();
    }
}
