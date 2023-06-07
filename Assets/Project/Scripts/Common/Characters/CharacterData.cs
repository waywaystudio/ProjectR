using System.Collections.Generic;
using Serialization;
using UnityEngine;

namespace Common.Characters
{
    // TODO. VillainData 와 VenturerData는 ScriptableObject를 상속받고, CharacterData 는 ICharacterData로 Interface 어떨까;
    public class CharacterData : ScriptableObject, ISavable, IEditable
    {
        [SerializeField] protected DataIndex characterIndex;
        [SerializeField] protected CharacterConstEntity constEntity;

        public DataIndex DataIndex => characterIndex;
        public VenturerType VenturerType => (VenturerType)characterIndex;
        public IEnumerable<DataIndex> SkillList => constEntity.DefaultSkillList;
        public StatTable StaticStatTable { get; } = new();
        

        public float GetStatValue(StatType type) => StaticStatTable.GetStatValue(type);
        public string GetStatTextValue(StatType type) => StaticStatTable.GetStatValue(type).ToStatUIValue(type);

        public virtual void Save() { }
        public virtual void Load()
        {
            StaticStatTable.Clear();
            StaticStatTable.Add(constEntity.DefaultStatSpec);
        }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            if (!Verify.IsNotDefault(characterIndex, "", false))
            {
                var soObjectNameToDatIndex = name.Replace("Data", "").ConvertDataIndexStyle();
                if (!DataIndex.TryFindDataIndex(soObjectNameToDatIndex, out characterIndex)) return;
            }
            
            constEntity.EditorSetUpByDataIndex(characterIndex);
        }
#endif
    }
}
