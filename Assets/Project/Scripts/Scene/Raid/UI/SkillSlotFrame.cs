using Character;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Raid.UI
{
    public class SkillSlotFrame : MonoBehaviour
    {
        // [SerializeField] private string hotkey;
        [SerializeField] private int skillIndex;
        [SerializeField] private Image globalCooldownFilter;
        [SerializeField] private Image skillCooldownFilter;
        [SerializeField] private Image skillImage;
        [SerializeField] private InputAction hotKeyAction;

        private int instanceID;
        private RaidUIDirector uiDirector;
        private ISkillInfo inheritSkill;
        private ISkillBehaviour combatBehaviour;

        private bool IsRegistered => inheritSkill != null;
        private bool hasCoolTimeEntity;


        public void UseSkill(InputAction.CallbackContext context)
        {
            // TODO. Use Skill of skillIndex;
            Debug.Log("UseSkill In");
        }

        public void Register(AdventurerBehaviour ab)
        {
            gameObject.SetActive(true);
            combatBehaviour = ab.SkillBehaviour;

            if (skillIndex >= combatBehaviour.SkillInfoList.Count)
            {
                Debug.LogWarning($"{ab.Name} has not enough skill count. "
                                 + $"required:{skillIndex + 1}, {ab.Name} has {combatBehaviour.SkillInfoList.Count} skills");
                return;
            }
            
            // Set Skill
            inheritSkill = combatBehaviour.SkillInfoList[skillIndex];
            hasCoolTimeEntity = inheritSkill is { HasCoolTimeModule: true };

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
            
            // Set Hotkey Action
            hotKeyAction.Enable();
            hotKeyAction.performed += UseSkill;
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
            
            // Release Hotkey Action
            hotKeyAction.performed -= UseSkill;
            hotKeyAction.Disable();
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
            uiDirector.SkillSlotFrameList.AddUniquely(this);
        }
    }
}
