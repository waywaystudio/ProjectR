using Sequences;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    [Button]
    public void Debugger()
    {
        var sequencer = GetComponent<Sequencer>();

        sequencer.Active();
    }
}
