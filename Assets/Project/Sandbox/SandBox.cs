using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour, INotifyPropertyChanged
{
    [Button]
    private void Debugger()
    {
        float thisIsFloat = 1f;
        float thisIsFloat2 = 0f;
        
        Debug.Log($"{thisIsFloat.GetHashCode()}{thisIsFloat2.GetHashCode()}");
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
