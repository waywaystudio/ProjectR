using System.Collections.Generic;
using Character.Bosses;
using Common;
using Common.Characters;
using Common.Equipments;
using Common.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class BossSelectFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private TextMeshProUGUI bossTitleUGUI;
        [SerializeField] private GameObject dropItemSlotPrefab;
        [SerializeField] private Transform dropItemsInfoUIHierarchy;
        [SerializeField] private List<CharacterData> bossDataList;
        [SerializeField] private List<GameObject> portraitList;
        [SerializeField] private List<Image> skillImageList;
        [SerializeField] private List<StatInfoUI> statInfoUIList;
        [SerializeField] private List<DropItemUI> dropItemUIList;

        private DataIndex selectedBossCode;
        private string GetPortraitObjectName(DataIndex bossCode) => $"{bossCode}Portrait";


        public void SetBoss(DataIndex bossCode)
        {
            SetPortrait(bossCode);

            if (!Database.BossPrefabData.Get(bossCode, out Boss bossBehaviour)) return;

            var bossData = bossDataList.TryGetElement(bossData => bossData.DataIndex == bossCode);

            selectedBossCode   = bossCode;
            bossTitleUGUI.text = bossBehaviour.Name;
            
            bossBehaviour.ForceInitialize();
            
            // SetStat
            GetStat(StatType.Power).SetValue(bossData.GetStatValue(StatType.Power).ToString("0"));
            GetStat(StatType.CriticalChance).SetValue($"{bossData.GetStatValue(StatType.CriticalChance):F1}%");
            GetStat(StatType.CriticalDamage).SetValue($"{200f + bossData.GetStatValue(StatType.CriticalDamage):F1}%");
            GetStat(StatType.Haste).SetValue($"{bossData.GetStatValue(StatType.Haste):F1}%");
            GetStat(StatType.Armor).SetValue(bossData.GetStatValue(StatType.Armor).ToString("0"));
            GetStat(StatType.Health).SetValue(bossData.GetStatValue(StatType.Health).ToString("0"));
            
            // SetSkillIcon
            bossBehaviour.SkillBehaviour.SkillList.ForEach((skill, index) =>
            {
                if (skillImageList.Count > index)
                {
                    skillImageList[index].sprite = skill.Icon;
                }
            });
            
            // SetDropItems
            var dropItemList = bossBehaviour.DropItemTable;
            
            dropItemUIList.TrimAndDestroy(dropItemList.Count);

            dropItemList.ForEach((itemObject, index) =>
            {
                if (!itemObject.TryGetComponent(out Equipment dropItem)) return;
                if (dropItemUIList.Count > index)
                {
                    dropItemUIList[index].SetDropItemUI(dropItem);
                }
                else
                {
                    var dropItemUIObject = Instantiate(dropItemSlotPrefab, dropItemsInfoUIHierarchy);

                    if (!dropItemUIObject.TryGetComponent(out DropItemUI dropItemUIBehaviour)) return;
                    
                    dropItemUIBehaviour.SetDropItemUI(dropItem);
                    dropItemUIList.Add(dropItemUIBehaviour);
                }
            });
        }

        public void GetNextBoss()
        {
            var currentData  = bossDataList.TryGetElement(bossData => bossData.DataIndex == selectedBossCode);
            var nextBossCode = bossDataList.GetNext(currentData);
            
            SetBoss(nextBossCode.DataIndex);
        }
        
        public void GetPreviousBoss()
        {
            var currentData  = bossDataList.TryGetElement(bossData => bossData.DataIndex == selectedBossCode);
            var previousBossCode = bossDataList.GetPrevious(currentData);
            
            SetBoss(previousBossCode.DataIndex);
        }


        // TODO.TEMP Initialize
        private void Awake()
        {
            selectedBossCode = DataIndex.Graevar;
            
            SetBoss(DataIndex.Graevar);
        }

        private void SetPortrait(DataIndex bossCode)
        {
            foreach (var portrait in portraitList)
            {
                if (portrait.name == GetPortraitObjectName(bossCode))
                {
                    portrait.SetActive(true);
                    continue;
                }
                
                portrait.SetActive(false);
            }
        }
        
        private StatInfoUI GetStat(StatType type) => statInfoUIList.TryGetElement(stat => stat.StatType == type);


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(statInfoUIList);
            GetComponentsInChildren(dropItemUIList);
        }
#endif
    }
}
