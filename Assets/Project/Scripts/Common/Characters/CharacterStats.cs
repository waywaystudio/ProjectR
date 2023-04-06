using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Characters
{
    public class CharacterStats : MonoBehaviour, IDynamicStatEntry, IEditable
    {
        [SerializeField] private DataIndex baseStatCode;
        [SerializeField] private Spec combatClassSpec = new();

        public AliveValue Alive { get; } = new();
        public HpValue Hp { get; } = new();
        public ResourceValue Resource { get; } = new();
        public ShieldValue Shield { get; } = new();
        
        [ShowInInspector] public StatTable StatTable { get; } = new();
        [ShowInInspector] public StatusEffectTable DeBuffTable { get; } = new();
        [ShowInInspector] public StatusEffectTable BuffTable { get; } = new();
        

        public void Initialize()
        {
            combatClassSpec.Register("CombatClassSpec", StatTable);
            
            Hp.StatTable       = StatTable;
            Resource.StatTable = StatTable;
            Shield.StatTable   = StatTable;

            SetDynamicStatEntry();
        }

        public void SetDynamicStatEntry()
        {
            Alive.Value    = true;
            Hp.Value       = StatTable.MaxHp;
            Resource.Value = StatTable.MaxResource;
            Shield.Value   = 0;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            baseStatCode = GetComponent<IDataIndexer>().ActionCode;

            combatClassSpec.Clear();

            switch (baseStatCode.GetCategory())
            {
                case DataIndex.CombatClass:
                {
                    var classData = Database.CombatClassSheetData(baseStatCode);

                    combatClassSpec.Add(StatType.CriticalChance, StatApplyType.Plus, classData.Critical);
                    combatClassSpec.Add(StatType.Haste,          StatApplyType.Plus, classData.Haste);
                    combatClassSpec.Add(StatType.Armor,          StatApplyType.Plus, classData.Armor);
                    combatClassSpec.Add(StatType.MaxHp,          StatApplyType.Plus, classData.MaxHp);
                    combatClassSpec.Add(StatType.MaxResource,    StatApplyType.Plus, classData.MaxResource);
                    combatClassSpec.Add(StatType.MoveSpeed,      StatApplyType.Plus, classData.MoveSpeed);
                    break;
                }
                case DataIndex.Boss:
                {
                    var bossData = Database.BossSheetData(baseStatCode);

                    combatClassSpec.Add(StatType.CriticalChance, StatApplyType.Plus, bossData.Critical);
                    combatClassSpec.Add(StatType.Haste,          StatApplyType.Plus, bossData.Haste);
                    combatClassSpec.Add(StatType.Armor,          StatApplyType.Plus, bossData.Armor);
                    combatClassSpec.Add(StatType.MaxHp,          StatApplyType.Plus, bossData.MaxHp);
                    combatClassSpec.Add(StatType.MaxResource,    StatApplyType.Plus, bossData.MaxResource);
                    combatClassSpec.Add(StatType.MoveSpeed,      StatApplyType.Plus, bossData.MoveSpeed);
                    break;
                }
                default:
                {
                    Debug.LogWarning($"DataIndex Error. Must be CombatClass or Boss. Input:{(DataIndex)((int)baseStatCode / 100000)}");
                    return;
                } 
            }
        }
#endif
    }
}
