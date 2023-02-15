using Character.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.ActionBars.CharacterSkills
{
    // Required SkillInfoPrefab;
    public class CharacterSkillActionIcon : MonoBehaviour
    {
        [SerializeField] private SkillComponent skillComponent;
        [SerializeField] private Image actionIcon;
        [SerializeField] private TextMeshProUGUI description;

        private Transform preTransform;

        public void Set(Sprite icon, string description)
        {
            actionIcon.sprite     = icon;
            this.description.text = description;
        }
        
        // OnMouseOver
    }
}
