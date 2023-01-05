using Character;
using Character.Combat;
using Character.Combat.Skill;
using UnityEngine;
using UnityEngine.UI;

namespace Scene.Raid.UI
{
    public class SkillSlotFrame : MonoBehaviour
    {
        [SerializeField] private int skillIndex;
        // [SerializeField] private string hotkey;
        [SerializeField] private Image globalCooldownFilter;
        [SerializeField] private Image skillCooldownFilter;
        [SerializeField] private Image skillImage;

        private int instanceID;
        private RaidUIDirector uiDirector;
        private BaseSkill inheritSkill;
        private CombatBehaviour combatBehaviour;

        private bool IsRegistered => inheritSkill != null;
        private bool HasCoolTimeEntity => inheritSkill != null && inheritSkill.CoolTimeEntity != null;
        
        
        public void Register(AdventurerBehaviour ab)
        {
            gameObject.SetActive(true);
            combatBehaviour = ab.CombatBehaviour;
            
            // Set Skill
            inheritSkill = combatBehaviour.SkillList[skillIndex];

            // Set SkillIcon
            skillImage.sprite = inheritSkill.Icon;
            
            // Set GlobalCooldown
            combatBehaviour.GlobalCoolDown.Timer.Register(instanceID, UnFillGlobalCoolTime);

            // Set SkillCoolTime
            switch (HasCoolTimeEntity)
            {
                case false:
                    skillCooldownFilter.fillAmount = 0.0f;
                    break;
                case true:
                {
                    var coolTimeEntity = inheritSkill.CoolTimeEntity;
                    coolTimeEntity.RemainTimer.Register(instanceID, UnFillSkillCoolTime);
                    break;
                }
            }
        }

        public void Unregister()
        {
            if (!IsRegistered) return;

            combatBehaviour.GlobalCoolDown.Timer.Unregister(instanceID);
            
            if (HasCoolTimeEntity) 
                inheritSkill.CoolTimeEntity.RemainTimer.Unregister(instanceID);
            
            inheritSkill = null;
            
            if (isActiveAndEnabled) 
                gameObject.SetActive(false);
        }
        

        private void UnFillGlobalCoolTime(float remainGlobalCoolTime)
        {
            if (HasCoolTimeEntity)
            {
                if (inheritSkill.CoolTimeEntity.RemainTimer.Value > combatBehaviour.GlobalCoolDown.CoolTime)
                {
                    globalCooldownFilter.fillAmount = 0.0f;
                    return;
                }
            }
            
            var normalGlobalCoolTime = remainGlobalCoolTime / combatBehaviour.GlobalCoolDown.CoolTime;
            
            globalCooldownFilter.fillAmount = normalGlobalCoolTime;
        }

        private void UnFillSkillCoolTime(float remainCoolTime)
        {
            if (inheritSkill.CoolTimeEntity.RemainTimer.Value < combatBehaviour.GlobalCoolDown.CoolTime)
            {
                skillCooldownFilter.fillAmount = 0.0f;
                return;
            }
            
            var normalCoolTime = remainCoolTime / inheritSkill.CoolTimeEntity.CoolTime;
            
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
