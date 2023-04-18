using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterStats : MonoBehaviour, IDynamicStatEntry, IEditable
    {
        [SerializeField] private CharacterData classSpecData;

        public AliveValue Alive { get; } = new();
        public HpValue Hp { get; } = new();
        public ResourceValue Resource { get; } = new();
        public ShieldValue Shield { get; } = new();
        public Spec CombatClassSpec => classSpecData.ClassSpec;
        
        [ShowInInspector] public StatTable StatTable { get; } = new();
        [ShowInInspector] public StatusEffectTable DeBuffTable { get; } = new();
        [ShowInInspector] public StatusEffectTable BuffTable { get; } = new();
        

        public void Initialize()
        {
            CombatClassSpec.Register("CombatClassSpec", StatTable);
            
            Hp.StatTable       = StatTable;
            Resource.StatTable = StatTable;
            Shield.StatTable   = StatTable;

            SetDynamicStatEntry();
        }

        public void SetDynamicStatEntry()
        {
            Alive.Value    = true;
            
            // Health 값이 모두 들어온 후에 * 10f 계산해야 하는데...
            Hp.Value       = StatTable.Health * 10f;
            Resource.Value = StatTable.MaxResource;
            Shield.Value   = 0;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            // TODO. GetCharacterData;
        }
#endif
    }
}
