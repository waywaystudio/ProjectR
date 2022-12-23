using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    public StatTable StatTable1 = new ();
    public StatTable StatTable2 = new ();
    
    [Button]
    private void Debugger()
    {
        StatTable1.Register(StatCode.AddPower, 1, () => 1f, true);
        StatTable1.Register(StatCode.AddPower, 2, () => 1f, true);
        StatTable2.Register(StatCode.AddPower, 3, () => 3f, true);
        StatTable2.Register(StatCode.AddPower, 4, () => 4f, true);

        Debug.Log($"{StatTable1.Power}, {StatTable2.Power} SUM : {StatTable1.Power + StatTable2.Power}");
        var newTable = StatTable1 + StatTable2;
        
        Debug.Log(newTable.Power);
    }
}
