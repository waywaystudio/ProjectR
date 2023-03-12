using Character;
using Common;
using UI.ActionBars;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.StageSetter
{
    public class PreviewActionSlot : ActionSlot, ITooltipInfo
    {
        [SerializeField] private Image iconImage;

        private SkillComponent targetSkill;
        public string TooltipInfo { get; set; }

        public void Initialize(SkillComponent skill)
        {
            targetSkill      = skill;
            iconImage.sprite = targetSkill.Icon;
            TooltipInfo      = targetSkill.Description;
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            iconImage = transform.Find("Icon").GetComponent<Image>();
        }
#endif
    }
}
