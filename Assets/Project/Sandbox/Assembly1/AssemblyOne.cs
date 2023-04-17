using UnityEngine;

public class AssemblyOne : MonoBehaviour
{
    public AssemblyOneInner Inner;

    public AssemblyOneInner GetInner() => Inner;
    
    public void ParamInner(AssemblyOneInner inner) => Debug.Log(inner.name);
    public void LogInner() => Debug.Log($"{Inner.ToString()}");
    public void ParamDirectInner() => ParamInner(Inner);
    public void RefInner()
    {
        AssemblyOneInner innerName = Inner;
        
        Debug.Log(innerName.name);
    }
}
