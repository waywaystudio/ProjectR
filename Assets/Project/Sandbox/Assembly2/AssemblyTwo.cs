using Sirenix.OdinInspector;
using UnityEngine;

public class AssemblyTwo : MonoBehaviour
{
    public AssemblyOne AssemblyOne;

    public void LogInner() => AssemblyOne.LogInner();
    public void RefInner() => AssemblyOne.RefInner();
    public void ParamDirectInner() => AssemblyOne.ParamDirectInner();

    /* Can not Reference Another Asmdef included parameter */
    // public void ParamInner() => AssemblyOne.ParamInner();
    
    /* Can not Reference Another Asmdef included ReturnType */
    // public void GetInner() => AssemblyOne.GetInner();
}
