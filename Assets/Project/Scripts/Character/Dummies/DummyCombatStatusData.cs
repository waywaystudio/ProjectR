using Common;
using UnityEngine;

namespace Character.Dummies
{
    public class DummyCombatStatusData : MonoBehaviour
    {
        [SerializeField] private StatSpec vummyStatSpec;
        
        public StatTable StatTable { get; } = new();

        public void Initialize()
        {
            StatTable.Add(vummyStatSpec);
        }
    }
}
