using UnityEngine;
using UnityGoogleSheet.Core.Type.Attribute;

namespace UnityGoogleSheet.Core
{
    [UGS(typeof(ESampleEnum))]
    public enum ESampleEnum
    {
	    Test, 
        Test2, 
        Test3
    }
}

public class SampleEnum : MonoBehaviour
{
    
}
