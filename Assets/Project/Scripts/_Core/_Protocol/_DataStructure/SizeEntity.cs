using System;
using UnityEngine;

[Serializable]
public struct SizeEntity
{
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;

    public void Set(Vector3 entity)
    {
        x = entity.x;
        y = entity.y;
        z = entity.z;
    }

    public SizeEntity(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public float PivotRange { get => x; set => x = value; }
    public float AreaRange { get => y; set => y = value; }
    public float Angle { get => z; set => z = value; }
    public float Width { get => z; set => z = value; }
    public float Height { get => y; set => y = value; }
    
    // For Donut Detector
    public float InnerRadius { get => y; set => y = value; }
    public float OuterRadius { get => z; set => z = value; }
}
