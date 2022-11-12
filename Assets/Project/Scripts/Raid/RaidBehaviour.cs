using System.Collections.Generic;
using UnityEngine;

namespace Raid
{
    public class RaidBehaviour : MonoBehaviour
    {
        private List<GameObject> testList = new ();
        
        private void Start()
        {
            AsyncTester(1000);
        }

        private void AsyncTester(int repeatCount)
        {
            for (var i = 0; i < repeatCount; ++i)
            {
                var dummy = Instantiate(new GameObject("Dummy"));
                testList.Add(dummy);
            }
            
            testList.ForEach(Destroy);
            testList.Clear();
            
            for (var i = 0; i < repeatCount; ++i)
            {
                var dummy = Instantiate(new GameObject("Dummy"));
                testList.Add(dummy);
            }
            
            testList.ForEach(Destroy);
            testList.Clear();
            
            for (var i = 0; i < repeatCount; ++i)
            {
                var dummy = Instantiate(new GameObject("Dummy"));
                testList.Add(dummy);
            }
            
            testList.ForEach(Destroy);
            testList.Clear();
            
            for (var i = 0; i < repeatCount; ++i)
            {
                var dummy = Instantiate(new GameObject("Dummy"));
                testList.Add(dummy);
            }
            
            testList.ForEach(Destroy);
            testList.Clear();
        }
    }
}
