using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Raid.UI.TutorialFrames
{
    public class TutorialClassUI : MonoBehaviour, IEditable
    {
        [SerializeField] private VenturerType venturerType;
        [SerializeField] private List<TutorialSkillUI> tutorialSkillUiList;

        public VenturerType VenturerType => venturerType;
        public bool IsAlreadyShown { get; private set; } = false;

        public void ShowVenturerTutorial()
        {
            if (IsAlreadyShown) return;

            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }

        public void OkToContinue()
        {
            Time.timeScale = 1f;
            IsAlreadyShown = true;
            gameObject.SetActive(false);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(tutorialSkillUiList);
            
            var venturerData = Camp.GetData(venturerType);
            var initialSkillList = venturerData.SkillList;

            tutorialSkillUiList.ForEach((skillUI, index) =>
            {
                skillUI.SetSkillInfo(initialSkillList[index]);
            });
        }
#endif
    }
}
