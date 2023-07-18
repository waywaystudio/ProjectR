using Common.StatusEffects;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterCombatStatus : MonoBehaviour
    {
        public AliveValue Alive { get; } = new();
        public HpValue Hp { get; } = new();
        public ResourceValue Resource { get; } = new();
        public ShieldValue Shield { get; } = new();
        public StatTable StatTable { get; } = new();
        public StatusEffectTable StatusEffectTable { get; } = new();

        public void Initialize(StatTable staticTable)
        {
            StatTable.RegisterTable(staticTable);
            
            Hp.StatTable       = StatTable;
            Resource.StatTable = StatTable;
            Shield.StatTable   = StatTable;
            Alive.Value        = true;
            Hp.Value           = StatTable.Health * 10f;
            Resource.Value     = StatTable.MaxResource;
            Shield.Value       = 0;
        }
        

        private void OnDestroy()
        {
            Alive.Clear();
            Hp.Clear();
            Resource.Clear();
            Shield.Clear();
            StatTable.Clear();
            StatusEffectTable.Clear();
        }
    }
}
