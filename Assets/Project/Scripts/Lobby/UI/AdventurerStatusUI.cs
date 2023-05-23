using System.Collections.Generic;
using Common;
using Common.PartyCamps;
using Common.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class AdventurerStatusUI : MonoBehaviour, IEditable
    {
        [SerializeField] private DataIndex dataIndex;
        [SerializeField] private TextMeshProUGUI className;
        [SerializeField] private List<StatInfoUI> statInfoList;
        [SerializeField] private List<Image> skillImageList;
        [SerializeField] private List<EquipmentInfoUI> equipmentInfoList;

        public DataIndex DataIndex => dataIndex;


        [Sirenix.OdinInspector.Button]
        public void Reload()
        {
            var adventurerData = PartyCamp.Characters.GetData(dataIndex);
            
            // Set Stat Value
            statInfoList.ForEach(statInfo =>
            {
                statInfo.SetValue(adventurerData.GetStatTextValue(statInfo.StatType));
            });
            
            // GetStat(StatType.Power).SetValue(adventurerData.GetStatValue(StatType.Power).ToString("0"));
            // GetStat(StatType.CriticalChance).SetValue($"{adventurerData.GetStatValue(StatType.CriticalChance):F1}%");
            // GetStat(StatType.CriticalDamage).SetValue($"{200f + adventurerData.GetStatValue(StatType.CriticalDamage):F1}%");
            // GetStat(StatType.Haste).SetValue($"{adventurerData.GetStatValue(StatType.Haste):F1}%");
            // GetStat(StatType.Armor).SetValue(adventurerData.GetStatValue(StatType.Armor).ToString("0"));
            // GetStat(StatType.Health).SetValue(adventurerData.GetStatValue(StatType.Health).ToString("0"));
            
            // Set Skill Icon
            adventurerData.SkillList.ForEach((dataIndex, index) =>
            {
                if (skillImageList.Count > index)
                {
                    skillImageList[index].sprite = Database.SpellSpriteData.Get(dataIndex);
                }
            });
            
            // Equipment Drawer
            adventurerData.EquipmentEntity.EquipmentEntities.ForEach(equipment =>
            {
                GetEquipmentUI(equipment.Key).SetEquipmentInfoUI(equipment.Value);
            });
        }


        private StatInfoUI GetStat(StatType type) => statInfoList.TryGetElement(stat => stat.StatType == type);
        private EquipmentInfoUI GetEquipmentUI(EquipSlotIndex type) => equipmentInfoList.TryGetElement(equip => equip.EquipSlot == type);

        private void Awake()
        {
            Reload();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            className = transform.Find("Portrait")
                                 .Find("Header")
                                 .Find("Title")
                                 .GetComponent<TextMeshProUGUI>();
            
            GetComponentsInChildren(true, statInfoList);
            GetComponentsInChildren(true, equipmentInfoList);

            var slotListTransform = transform.Find("Portrait")
                                             .Find("Contents")
                                             .Find("SlotList");
            
            skillImageList.Clear();
            skillImageList.Add(slotListTransform.Find("Slot 1").Find("Contents").GetComponent<Image>());
            skillImageList.Add(slotListTransform.Find("Slot 2").Find("Contents").GetComponent<Image>());
            skillImageList.Add(slotListTransform.Find("Slot 3").Find("Contents").GetComponent<Image>());
            skillImageList.Add(slotListTransform.Find("Slot 4").Find("Contents").GetComponent<Image>());
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
