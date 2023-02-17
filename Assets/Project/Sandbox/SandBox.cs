using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class SandBox : MonoBehaviour
{
    public UnityEvent TestEvent;

    [Button]
    public void Debugger()
    {
        TestEvent.Invoke();
    }

    [Button] public void AddAFunction() => TestEvent.AddListener(A);
    [Button] public void AddBFunction() => TestEvent.AddListener(B);
    [Button] public void AddCFunction() => TestEvent.AddListener(C);
    [Button] public void AddDFunction() => TestEvent.AddListener(D);
    [Button] public void RemoveAFunction() => TestEvent.RemoveListener(A);
    [Button] public void RemoveBFunction() => TestEvent.RemoveListener(B);
    [Button] public void RemoveCFunction() => TestEvent.RemoveListener(C);
    [Button] public void RemoveDFunction() => TestEvent.RemoveListener(D); 

    public void A() { Debug.Log("A");}
    public void B() { Debug.Log("B");}
    public void C() { Debug.Log("C");}
    public void D() { Debug.Log("D");}
}
