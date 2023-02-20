using Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public ActionTable ActionTable = new();

    public void ShowDebugMessage() => Debug.Log("Is In!");

    [Button]
    public void Debugger()
    {
        ActionTable.Invoke();
    }
    public void A() { Debug.Log("A");}
    public void B() { Debug.Log("B");}
    public void C() { Debug.Log("C");}
    public void D() { Debug.Log("D");}

    private void Awake()
    {
        ActionTable.Register("A", A);
        ActionTable.Register("B", B);
        ActionTable.Register("C", C);
        ActionTable.Register("D_", D);
    }
}
