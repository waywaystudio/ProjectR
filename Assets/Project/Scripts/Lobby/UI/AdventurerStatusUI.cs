using System.Collections.Generic;
using Common;
using Common.PlayerCamps;
using Common.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class AdventurerStatusUI : MonoBehaviour, IEditable
    {
        [SerializeField] private CombatClassType classType;
        [SerializeField] private TextMeshProUGUI className;
        [SerializeField] private List<StatInfoUI> statInfoList;
        [SerializeField] private List<Image> skillImageList;
        [SerializeField] private List<EquipmentInfoUI> equipmentInfoList;

        public CombatClassType ClassType => classType;


        [Sirenix.OdinInspector.Button]
        public void Reload()
        {
            var adventurer     = PlayerCamp.Characters.Get(classType);
            var adventurerData = PlayerCamp.Characters.GetData(classType);

            className.text = classType.ToString();
            
            GetStat(StatType.Power).SetValue(adventurerData.GetStatValue(StatType.Power).ToString("0"));
            GetStat(StatType.CriticalChance).SetValue($"{adventurerData.GetStatValue(StatType.CriticalChance):F1}%");
            GetStat(StatType.CriticalDamage).SetValue($"{200f + adventurerData.GetStatValue(StatType.CriticalDamage):F1}%");
            GetStat(StatType.Haste).SetValue($"{adventurerData.GetStatValue(StatType.Haste):F1}%");
            GetStat(StatType.Armor).SetValue(adventurerData.GetStatValue(StatType.Armor).ToString("0"));
            GetStat(StatType.Health).SetValue(adventurerData.GetStatValue(StatType.Health).ToString("0"));
            
            adventurer.SkillBehaviour.SkillList.ForEach((skill, index) =>
            {
                if (skillImageList.Count > index) skillImageList[index].sprite = skill.Icon;
            });
            
            adventurerData.Table.ForEach(equipment =>
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
