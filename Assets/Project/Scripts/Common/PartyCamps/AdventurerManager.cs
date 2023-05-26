using System.Collections.Generic;
using Common.Characters;
using Common.Equipments;
using Serialization;
using UnityEngine;

namespace Common.PartyCamps
{
    public class AdventurerManager : MonoBehaviour, ISavable, IEditable
    {
        [SerializeField] private List<CharacterData> characterDataList;

        public List<CharacterData> GetAllData() => characterDataList;
        public CharacterData GetData(CombatClassType                   type) => characterDataList.TryGetElement(data => data.ClassType == type);
        public CharacterData GetData(DataIndex                         type) => characterDataList.TryGetElement(data => data.DataIndex == type);
        public CharacterData GetNextData(CharacterData                 currentData) => characterDataList.GetNext(currentData);
        public CharacterData GetPreviousData(CharacterData             currentData) => characterDataList.GetPrevious(currentData);
        public EquipmentEntity GetAdventurerEquipment(CombatClassType  adventurer, EquipSlotIndex slot) => GetData(adventurer).GetEquipment(slot);
        public RelicEntity GetAdventurerEquipmentRelic(CombatClassType adventurer, EquipSlotIndex slot, RelicType relicType) => GetData(adventurer).GetEquipment(slot).GetRelic(relicType);

        public void Save() => characterDataList.ForEach(data => data.Save());
        public void Load() => characterDataList.ForEach(data => data.Load());


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            // TODO. Get AdventurerData and Sort by CombatClass Order.
        }
#endif
    }
}
