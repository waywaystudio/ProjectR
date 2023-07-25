using System.Collections;
using System.Collections.Generic;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandSphere : MonoBehaviour
{
    [ShowInInspector]
    public CombatEntity Entity;

    [Button]
    private void SetEntity()
    {
        // Entity = new CombatEntity();
    }

    [Button]
    private void EmptyEntity() => Entity = null;
}
