using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour, INotifyPropertyChanged
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
