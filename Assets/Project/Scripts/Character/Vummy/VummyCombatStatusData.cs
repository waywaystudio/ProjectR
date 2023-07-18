using Common;
using UnityEngine;

namespace Character.Vummy
{
    public class VummyCombatStatusData : MonoBehaviour
    {
        [SerializeField] private StatSpec vummyStatSpec;
        
        public StatTable StatTable { get; } = new();

        public void Initialize()
        {
            StatTable.Add(vummyStatSpec);
        }
    }
}
