using Common.Character;
using Common.Character.Operation;
using Common.Character.Operation.Combat;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI
{
    public class SkillSlotFrame : MonoBehaviour
    {
        [SerializeField] private int skillIndex;
        [SerializeField] private string hotkey;
        [SerializeField] private Image globalCooldownFilter;
        [SerializeField] private Image skillCooldownFilter;
        [SerializeField] private Image skillImage;

        private int instanceID;
        private RaidUIDirector uiDirector;
        private BaseSkill inheritSkill;
        private CombatOperation combatOperation;

        private bool IsRegistered => inheritSkill != null;
        private bool HasCoolTimeEntity => inheritSkill != null && inheritSkill.CoolTimeEntity != null;
        
        
        public void Register(AdventurerBehaviour ab)
        {
            gameObject.SetActive(true);
            combatOperation = ab.CombatOperation;
            
            // Set Skill
            switch (skillIndex)
            {
                case 1 : inheritSkill = combatOperation.FirstSkill; break;
                case 2 : inheritSkill = combatOperation.SecondSkill; break;
                case 3 : inheritSkill = combatOperation.ThirdSkill; break;
                case 4 : inheritSkill = combatOperation.FourthSkill; break;
                default:
                {
                    Debug.LogError($"Slot Index must in ranged 1 ~ 4. current:{skillIndex}");
                    break;
                }
            }
            
            // Set SkillIcon
            skillImage.sprite = inheritSkill.Icon;
            
            // Set GlobalCooldown
            combatOperation.GlobalCoolDown.OnTimerChanged.Register(instanceID, UnFillGlobalCoolTime);

            // Set SkillCoolTime
            switch (HasCoolTimeEntity)
            {
                case false:
                    skillCooldownFilter.fillAmount = 0.0f;
                    break;
                case true:
                {
                    var coolTimeEntity = inheritSkill.CoolTimeEntity;
                    coolTimeEntity.OnRemainTimeChanged.Register(instanceID, UnFillSkillCoolTime);
                    break;
                }
            }
        }

        public void Unregister()
        {
            if (!IsRegistered) return;

            combatOperation.GlobalCoolDown.OnTimerChanged.Unregister(instanceID);
            if (HasCoolTimeEntity) inheritSkill.CoolTimeEntity.OnRemainTimeChanged.Unregister(instanceID);
            inheritSkill = null;
            if (isActiveAndEnabled) gameObject.SetActive(false);
        }
        

        private void UnFillGlobalCoolTime(float remainGlobalCoolTime)
        {
            if (HasCoolTimeEntity)
            {
                if (inheritSkill.CoolTimeEntity.RemainTimer > combatOperation.GlobalCoolDown.CoolTime)
                {
                    globalCooldownFilter.fillAmount = 0.0f;
                    return;
                }
            }
            
            var normalGlobalCoolTime = remainGlobalCoolTime / combatOperation.GlobalCoolDown.CoolTime;
            
            globalCooldownFilter.fillAmount = normalGlobalCoolTime;
        }

        private void UnFillSkillCoolTime(float remainCoolTime)
        {
            if (inheritSkill.CoolTimeEntity.RemainTimer < combatOperation.GlobalCoolDown.CoolTime)
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

#if UNITY_EDITOR
        private void SetUp()
        {

        }
#endif
    }
}
