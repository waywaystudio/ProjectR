using Character;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Scene.Raid.UI
{
    public class SkillSlotFrame : MonoBehaviour
    {
        // [SerializeField] private string hotkey;
        [SerializeField] private int skillIndex;
        [SerializeField] private Image globalCooldownFilter;
        [SerializeField] private Image skillCooldownFilter;
        [SerializeField] private Image skillImage;

        private int instanceID;
        private RaidUIDirector uiDirector;
        private ISkillInfo inheritSkill;
        private ICombatBehaviour combatBehaviour;

        private bool IsRegistered => inheritSkill != null;
        private bool hasCoolTimeEntity;
        
        
        public void Register(AdventurerBehaviour ab)
        {
            gameObject.SetActive(true);
            combatBehaviour = ab.CombatBehaviour;
            
            // Set Skill
            inheritSkill = combatBehaviour.SkillInfoList[skillIndex];
            hasCoolTimeEntity = inheritSkill is { HasCoolTimeEntity: true };

            // Set SkillIcon
            skillImage.sprite = inheritSkill.Icon;
            
            // Set GlobalCooldown
            combatBehaviour.GlobalRemainTime.Register(instanceID, UnFillGlobalCoolTime);

            // Set SkillCoolTime
            switch (hasCoolTimeEntity)
            {
                case false:
                    skillCooldownFilter.fillAmount = 0.0f;
                    break;
                case true:
                {
                    inheritSkill.RemainTime.Register(instanceID, UnFillSkillCoolTime);
                    break;
                }
            }
        }

        public void Unregister()
        {
            if (!IsRegistered) return;

            combatBehaviour.GlobalRemainTime.Unregister(instanceID);
            
            if (hasCoolTimeEntity) 
                inheritSkill.RemainTime.Unregister(instanceID);
            
            inheritSkill = null;
            
            if (isActiveAndEnabled) 
                gameObject.SetActive(false);
        }
        

        private void UnFillGlobalCoolTime(float remainGlobalCoolTime)
        {
            if (hasCoolTimeEntity)
            {
                if (inheritSkill.RemainTime.Value > combatBehaviour.GlobalCoolTime)
                {
                    globalCooldownFilter.fillAmount = 0.0f;
                    return;
                }
            }
            
            var normalGlobalCoolTime = remainGlobalCoolTime / combatBehaviour.GlobalCoolTime;
            
            globalCooldownFilter.fillAmount = normalGlobalCoolTime;
        }

        private void UnFillSkillCoolTime(float remainCoolTime)
        {
            if (inheritSkill.RemainTime.Value < combatBehaviour.GlobalCoolTime)
            {
                skillCooldownFilter.fillAmount = 0.0f;
                return;
            }
            
            var normalCoolTime = remainCoolTime / inheritSkill.CoolTime;
            
            skillCooldownFilter.fillAmount = normalCoolTime;
        }
        

        private void Awake()
        {
            instanceID = GetInstanceID();
            uiDirector = GetComponentInParent<RaidUIDirector>();
            uiDirector.SkillSlotFrameList.Add(this);
        }
    }
}
