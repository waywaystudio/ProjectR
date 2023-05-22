using Serialization;
using UnityEngine;

namespace Common.Characters
{
    /*
     * CharacterData 는 Prefab과 UI에서 사용하는 핵심 데이타.
     * 따라서 편의성 함수들이 존재 해야함.
     */
    public class CharacterData : ScriptableObject, ISavable, IEditable
    {
        [SerializeField] private DataIndex characterIndex;
        [SerializeField] private CharacterConstEntity constEntity;
        [SerializeField] private CharacterEquipmentEntity equipmentEntity;

        public DataIndex DataIndex => characterIndex;
        public CombatClassType ClassType => constEntity.ClassType;
        public CharacterConstEntity ConstEntity => constEntity;
        public CharacterEquipmentEntity EquipmentEntity => equipmentEntity;

        [Sirenix.OdinInspector.ShowInInspector]
        public StatTable StaticStatTable { get; set; } = new();


        public float GetStatValue(StatType type) => StaticStatTable.GetStatValue(type);
        public void Save()
        {
            equipmentEntity.Save(characterIndex.ToString());
        }

        public void Load()
        {
            equipmentEntity.Load(characterIndex.ToString());
            
            StaticStatTable.Clear();
            StaticStatTable.Add(ConstEntity.DefaultSpec);
            StaticStatTable.Add(EquipmentEntity.EquipmentsStatTable);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (!Verify.IsNotDefault(characterIndex, "", false))
            {
                var soObjectNameToDatIndex = name.Replace("Data", "").ConvertDataIndexStyle();
                if (!DataIndex.TryFindDataIndex(soObjectNameToDatIndex, out characterIndex)) return;
            }
            
            constEntity.EditorSetUpByDataIndex(characterIndex);
            equipmentEntity.EditorSetUpByDataIndex(characterIndex);
        }
#endif
    }
}
