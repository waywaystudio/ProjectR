using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CoroutineTester : MonoBehaviour
{
    [PropertyRange(0, 10)]
    [SerializeField] private int testerValue;
    
    [Button]
    public void CoroutineTest()
    {
        
    }
}
