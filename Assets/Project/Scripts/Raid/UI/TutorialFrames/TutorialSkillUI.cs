using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.TutorialFrames
{
    public class TutorialSkillUI : MonoBehaviour, IEditable
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameTextMesh;
        [SerializeField] private TextMeshProUGUI coolTimeTextMesh;
        [SerializeField] private TextMeshProUGUI typeTextMesh;
        [SerializeField] private TextMeshProUGUI resourceTextMesh;
        [SerializeField] private TextMeshProUGUI descriptionTextMesh;

        public void SetSkillInfo(DataIndex skillCode)
        {
            var skillData = Database.SkillSheetData(skillCode);
            var spellData = Database.SpellSpriteData.Get(skillCode);

            iconImage.sprite         = spellData;
            nameTextMesh.text        = skillData.Name.ToDivideWords();
            coolTimeTextMesh.text    = skillData.CoolTime.ToString("F1");
            typeTextMesh.text        = skillData.SkillType;
            resourceTextMesh.text    = skillData.Cost.ToString("F1");
            descriptionTextMesh.text = skillData.Description;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            iconImage           = transform.Find("Contents").Find("Skill Icon").GetComponent<Image>();
            nameTextMesh        = transform.Find("Contents").Find("Skill Name").GetComponent<TextMeshProUGUI>();
            coolTimeTextMesh    = transform.Find("Contents").Find("Skill CoolTime").GetComponent<TextMeshProUGUI>();
            typeTextMesh        = transform.Find("Contents").Find("Skill Type").GetComponent<TextMeshProUGUI>();
            resourceTextMesh    = transform.Find("Contents").Find("Skill Resource").GetComponent<TextMeshProUGUI>();
            descriptionTextMesh = transform.Find("Contents").Find("Skill Description").GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
