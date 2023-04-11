using System.Collections.Generic;
using Common;
using Common.PlayerCamps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class AdventurerStatusUI : MonoBehaviour, IEditable
    {
        [SerializeField] private CombatClassType classType;
        [SerializeField] private List<StatInfoUI> statInfoList;
        [SerializeField] private List<Image> skillImageList;
        [SerializeField] private List<EquipmentInfoUI> equipmentInfoList;


        [Sirenix.OdinInspector.Button]
        public void Reload()
        {
            var adventurer = PlayerCamp.Characters.Get(classType);

            // TODO. StatTable값이 안들어와 있을 수 있다.
            GetStat(StatType.Power).SetValue(adventurer.StatTable.Power.ToString("0"));
            GetStat(StatType.CriticalChance).SetValue($"{adventurer.StatTable.CriticalChance:F1}%");
            GetStat(StatType.CriticalDamage).SetValue($"{200f + adventurer.StatTable.CriticalDamage:F1}%");
            GetStat(StatType.Haste).SetValue($"{adventurer.StatTable.Haste:F1}%");
            GetStat(StatType.Armor).SetValue(adventurer.StatTable.Armor.ToString("0"));
            GetStat(StatType.Health).SetValue(adventurer.StatTable.Health.ToString("0"));
            
            adventurer.SkillBehaviour.SkillList.ForEach((skill, index) =>
            {
                if (skillImageList.Count > index) skillImageList[index].sprite = skill.Icon;
            });
            
            adventurer.Equipment.Table.ForEach(equipment =>
            {
                GetEquipmentUI(equipment.Key).SetEquipmentInfoUI(equipment.Value.Info);
            });
        }


        private StatInfoUI GetStat(StatType type) => statInfoList.Find(stat => stat.StatType == type);
        private EquipmentInfoUI GetEquipmentUI(EquipSlotIndex type) => equipmentInfoList.Find(equip => equip.EquipSlot == type);

        private void Awake()
        {
            Reload();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            var className = transform.Find("Portrait")
                                     .Find("Header")
                                     .Find("Title")
                                     .GetComponent<TextMeshProUGUI>();

            className.text = classType.ToString();
            
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
        }
#endif
    }
}
