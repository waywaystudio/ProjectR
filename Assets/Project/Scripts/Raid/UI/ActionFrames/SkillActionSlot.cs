using Character.Adventurers;
using Common.Actions;
using Common.Skills;
using Common.UI;
using Manager;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Raid.UI.ActionFrames
{
    public class SkillActionSlot : MonoBehaviour
    {
        [SerializeField] private BindingCode bindingCode;
        [SerializeField] private TextMeshProUGUI hotKey;
        [SerializeField] private SkillActionSymbol skillAction;
        [SerializeField] private ImageFiller coolDownFiller;
        
        [PropertyRange(1, 4)]
        [SerializeField] private int skillIndex;

        private Adventurer focusedAdventurer;
        private DataIndex actionCode;

        private string HotKey =>
            bindingCode switch
            {
                BindingCode.Q => "Q",
                BindingCode.W => "W",
                BindingCode.E => "E",
                BindingCode.R => "R",
                _ => "-",
            };

        public void Initialize(ActionBehaviour actionBehaviour, SkillComponent skill)
        {
            // skillAction.Initialize(actionBehaviour, skill);
        }

        public void Initialize(Adventurer adventurer) => OnFocusChanged(adventurer);
        public void OnFocusChanged(Adventurer ab)
        {
            MainManager.Input.TryGetAction(bindingCode, out var inputAction);
            
            inputAction.started  -= skillAction.StartAction;
            inputAction.canceled -= skillAction.ReleaseAction;
            
            coolDownFiller.ProgressImage.enabled = false;

            if (ab.IsNullOrEmpty()) return;
            
            focusedAdventurer = ab;
            
            var actionBehaviour = focusedAdventurer.CharacterAction;
            var skill = skillIndex switch
            {
                1 => actionBehaviour.FirstSkill,
                2 => actionBehaviour.SecondSkill,
                3 => actionBehaviour.ThirdSkill,
                4 => actionBehaviour.FourthSkill,
                _ => null,
            };

            if (skill is null)
            {
                Debug.LogWarning($"Out of Range Skill or Character Skill Index Missing. UIIndexInput:{skillIndex}");
                return;
            }
            
            // skillAction.Initialize(ab, skill);

            if (skill.CoolTime != 0.0f)
            {
                coolDownFiller.ProgressImage.enabled = true;
                coolDownFiller.Register(skill.RemainCoolTime, skill.CoolTime);
            }

            inputAction.started  += skillAction.StartAction;
            inputAction.canceled += skillAction.ReleaseAction;
        }

        private void Awake()
        {
            skillAction ??= GetComponentInChildren<SkillActionSymbol>();
            hotKey.text =   HotKey;
        }


        public void SetUp()
        {
            skillAction ??= GetComponentInChildren<SkillActionSymbol>();
            hotKey.text =   HotKey;
        }
    }
}
