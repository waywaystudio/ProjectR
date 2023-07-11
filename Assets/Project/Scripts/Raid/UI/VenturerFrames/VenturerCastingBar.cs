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

        private bool isActive;
        private VenturerBehaviour currentVenturer;
        private static VenturerBehaviour FocusVenturer => RaidDirector.FocusVenturer;


        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            if (currentVenturer != null)
            {
                currentVenturer.SkillBehaviour.SkillIndexList.ForEach(index =>
                {
                    var skill = currentVenturer.GetSkill(index);
                    
                    skill.Builder
                         .Remove(Section.Active, "ShowCastingUI")
                         .Remove(Section.Execute, "HideCastingUI")
                         .Remove(Section.End, "HideCastingUI");
                });

                HideCastingUI();
            }
            
            FocusVenturer.SkillBehaviour.SkillIndexList.ForEach(index =>
            {
                var skill = FocusVenturer.GetSkill(index);
                    
                skill.Builder
                     .Add(Section.Active, "ShowCastingUI", ShowCastingUI)
                     .Add(Section.Execute, "HideCastingUI", HideCastingUI)
                     .Add(Section.End, "HideCastingUI", HideCastingUI);
            });

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

            isActive = true;
        }

        private void HideCastingUI()
        {
            if (!isActive) return;
            
            progressBar.Unregister();
            progressObject.SetActive(false);
            skillNameObject.SetActive(false);
            skillNameTextMesh.text = "";
            
            isActive = false;
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
