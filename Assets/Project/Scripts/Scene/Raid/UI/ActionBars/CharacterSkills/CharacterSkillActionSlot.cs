using Character;
using Core;
using MainGame;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Raid.UI.ActionBars.CharacterSkills
{
    public class CharacterSkillActionSlot : MonoBehaviour
    {
        [SerializeField] private CharacterSkillActionIcon skillAction;
        [SerializeField] private ImageFiller coolDownFiller;
        [SerializeField] private TextMeshProUGUI hotKey;
        [SerializeField] private BindingCode bindingCode;
        [PropertyRange(1, 4)]
        [SerializeField] private int skillIndex;

        private AdventurerBehaviour focusedAdventurer;
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


        public void Initialize(AdventurerBehaviour adventurer) => OnFocusChanged(adventurer);
        public void OnFocusChanged(AdventurerBehaviour ab)
        {
            MainManager.Input.TryGetAction(bindingCode, out var inputAction);
            
            inputAction.started  -= skillAction.StartAction;
            inputAction.canceled -= skillAction.CompleteAction;
            
            focusedAdventurer = ab;
            
            var actionBehaviour = focusedAdventurer.ActionBehaviour;
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
            
            skillAction.Initialize(ab, skill);

            if (skill.CoolTime == 0.0f)
            {
                coolDownFiller.ProgressImage.enabled = false;
            }
            else
            {
                coolDownFiller.ProgressImage.enabled = true;
                coolDownFiller.Register(skill.RemainCoolTime, skill.CoolTime);
            }
                
            
            inputAction.started  += skillAction.StartAction;
            inputAction.canceled += skillAction.CompleteAction;
        }

        private void Awake()
        {
            skillAction ??= GetComponentInChildren<CharacterSkillActionIcon>();
            hotKey.text =   HotKey;
        }


        public void SetUp()
        {
            skillAction ??= GetComponentInChildren<CharacterSkillActionIcon>();
            hotKey.text =   HotKey;
        }
    }
}
