using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public SandSphere sandSphere;


    public void ShowDebugMessage() => Debug.Log("Is In!");

    [Button]
    public void Debugger()
    {
        
    }
    public void A() { Debug.Log("A");}
    public void B() { Debug.Log("B");}
    public void C() { Debug.Log("C");}
    public void D() { Debug.Log("D");}

    // private void Awake()
    // {
    //     TryGetComponent(out sandSphere);
    //     
    //     ActionTable.Register("A", A);
    //     ActionTable.Register("B", B);
    //     ActionTable.Register("C", C);
    //     ActionTable.Register("D_", D);
    // }
}
