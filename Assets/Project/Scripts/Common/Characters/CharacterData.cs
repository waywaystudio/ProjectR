using System.Collections.Generic;
using Serialization;
using UnityEngine;

namespace Common.Characters
{
    // TODO. CharacterData 는 ICharacterData로 Interface 어떨까;
    public class CharacterData : ScriptableObject, ISavable, IDataIndexer, IEditable
    {
        [SerializeField] protected DataIndex characterIndex;
        [SerializeField] protected CharacterMask mask;
        [SerializeField] protected CharacterConstEntity constEntity;

        public DataIndex DataIndex => characterIndex;
        public CharacterMask CharacterMask => mask;
        public IEnumerable<DataIndex> SkillList => constEntity.DefaultSkillList;
        public string Name => constEntity.CharacterName;
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
