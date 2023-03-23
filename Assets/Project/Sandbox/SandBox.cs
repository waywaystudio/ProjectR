using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    // public Action CsharpAction;
    // public SandSphere sandSphere;

    // [SerializeField] private Processor processor = new();

    public void ShowDebugMessage() => Debug.Log("Is In!");

    [Button]
    public void Debugger()
    {
        // if (!componentA.TryGetComponent(out IProgress progressA)) return;
        // if (!componentB.TryGetComponent(out IProgress progressB)) return;
        // if (!componentC.TryGetComponent(out IProgress progressC)) return;
        
        // Debug.Log(progressA.CastingTime);
        // Debug.Log(progressB.CastingTime);
        // Debug.Log(progressC.CastingTime);
        
        // ShowDebugMessage();
        // processor.Activate(3f);
    }

    // [Button]
    // public void SetProgress(float value) => processor.Value = value;
    //
    // [Button]
    // public void SetEndTime(float value) => processor.EndTime = (value);
    //
    // [Button]
    // public void GetProgress()
    // {
    //     Debug.Log(processor.Value);
    // }
    
    public void A() { Debug.Log("A");}
    public void B() { Debug.Log("B");}
    public void C() { Debug.Log("C");}
    public void D() { Debug.Log("D");}
}
