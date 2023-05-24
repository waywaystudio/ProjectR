using UnityEngine;

namespace Common.Characters
{
    public class CharacterCombatStatus : MonoBehaviour, IDynamicStatEntry, IEditable
    {
        [SerializeField] private CharacterData characterData;

        public AliveValue Alive { get; } = new();
        public HpValue Hp { get; } = new();
        public ResourceValue Resource { get; } = new();
        public ShieldValue Shield { get; } = new();
        public StatTable StatTable { get; } = new();
        public StatusEffectTable DeBuffTable { get; } = new();
        public StatusEffectTable BuffTable { get; } = new();
        

        public void Initialize()
        {
            StatTable.Clear();
            StatTable.RegisterTable(characterData.StaticStatTable);
            // Add More Static Specs and StatTable such as, PlayerCamp Buff

            Hp.StatTable       = StatTable;
            Resource.StatTable = StatTable;
            Shield.StatTable   = StatTable;
            Alive.Value        = true;
            Hp.Value           = StatTable.Health * 10f;
            Resource.Value     = StatTable.MaxResource;
            Shield.Value       = 0;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            characterData.EditorSetUp();
        }
#endif
    }
}
