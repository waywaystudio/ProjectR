using Common.Character;
using Common.Character.Operation.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI
{
    public class SkillSlotFrame : MonoBehaviour
    {
        [SerializeField] private string hotkey;
        [SerializeField] private Image cooldownFilter;

        private RaidUIDirector uiDirector;
        
        // 게임오브젝트를 받아야 되나...
        // public AdventurerBehaviour AdventurerBehaviour { get; private set; }
        public BaseSkill Skill { get; set; }
        public Combating Combat { get; set; }

        public void Initialize(AdventurerBehaviour ab)
        {
            Combat = ab.GetComponentInChildren<Combating>();
            
            var currentRemainCoolTime = Combat.GlobalCoolTimer;
            var normalGlobalCoolTime = currentRemainCoolTime / Combat.GlobalCoolTime;

            // Skill = skill;

            // Set SkillIcon
            // Set SkillCurrentCooldown to Cooldown;
            // Set GlobalCooldown to GlobalCooldown;

            // ab.Status.OnHpChanged.Register(instanceID, FillHealthBar);
            // ab.Status.OnResourceChanged.Register(instanceID, FillResourceBar);

            // FillHealthBar(ab.Status.Hp);
            // FillResourceBar(ab.Status.Resource);

            // OnInitialize.Invoke();
        }
    }
}
