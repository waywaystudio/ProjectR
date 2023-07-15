using System;
using UnityEngine;

/// <summary>
/// 계수를 적용하는 값 형태 변수(가속도, 쿨타임 등)의 "계수"사용성을 위해 만들어진 클래스.
/// 기본적으로 1f 를 반환하며, 생성자에서 계수를 참조형태로 전달한다. 
/// </summary>
public class ValueWeight
{
    public ValueWeight() : this(null) { }
    public ValueWeight(Func<float> weight)
    {
        Weight = weight;
    }
    
    private Func<float> Weight { get; set; }
    
    /// <summary>
    /// 참조계수가 없으면 1f를 반환한다.
    /// </summary>
    /// <returns>참조계수가 없으면 1f를 반환</returns>
    public float Invoke()
    {
        if (Weight is null) return 1f;

        var valueWeight = Weight.Invoke();

        if (valueWeight != 0f) return 1f * valueWeight;
        
        Debug.LogWarning("Weight is 0. return 0;");
        return 0f;
    }
}
