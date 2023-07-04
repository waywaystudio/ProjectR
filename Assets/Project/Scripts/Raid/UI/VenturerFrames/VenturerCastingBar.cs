using Character.Venturers;
using Common.UI;
using TMPro;
using UnityEngine;

namespace Raid.UI.VenturerFrames
{
    public class VenturerCastingBar : MonoBehaviour, IEditable
    {
        [SerializeField] private ImageFiller progressBar;
        [SerializeField] private TextMeshProUGUI skillNameTextMesh;
        [SerializeField] private GameObject progressObject;
        [SerializeField] private GameObject skillNameObject;

        private VenturerBehaviour currentVenturer;
        private static VenturerBehaviour FocusVenturer => RaidDirector.FocusVenturer;


        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            if (currentVenturer != null)
            {
                currentVenturer.SkillBehaviour.SequenceBuilder
                                .Remove(SectionType.Active, "ShowCastingUI")
                                .Remove(SectionType.End, "HideCastingUI");

                HideCastingUI();
            }

            FocusVenturer.SkillBehaviour.SequenceBuilder
                         .Add(SectionType.Active, "ShowCastingUI", ShowCastingUI)
                         .Add(SectionType.End, "HideCastingUI", HideCastingUI);

            currentVenturer = FocusVenturer;
            ShowCastingUI();
        }


        private void ShowCastingUI()
        {
            var currentSkill = FocusVenturer.SkillBehaviour.Current;
            
            if (currentSkill is null) return;
            if (currentSkill.CastingWeight == 0f) return;
            
            progressObject.SetActive(true);
            skillNameObject.SetActive(true);
            progressBar.Register(currentSkill.CastTimer.EventTimer, currentSkill.CastTimer.CastingTime);
            skillNameTextMesh.text = currentSkill.DataIndex.ToString().ToDivideWords();
        }

        private void HideCastingUI()
        {
            progressBar.Unregister();
            progressObject.SetActive(false);
            skillNameObject.SetActive(false);
            skillNameTextMesh.text = "";
        }



#if UNITY_EDITOR
        public void EditorSetUp()
        {
            progressBar       = transform.Find("ProgressBar").Find("Bar").GetComponent<ImageFiller>();
            skillNameTextMesh = transform.Find("CastingInfo").Find("SkillName").GetComponent<TextMeshProUGUI>();
            progressObject    = transform.Find("ProgressBar").gameObject;
            skillNameObject   = transform.Find("CastingInfo").gameObject;
        }
#endif
    }
}
