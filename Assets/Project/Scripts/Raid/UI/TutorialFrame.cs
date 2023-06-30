using Character.Venturers;
using Common;
using Raid.UI.TutorialFrames;
using UnityEngine;

namespace Raid.UI
{
    public class TutorialFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private Table<VenturerType, TutorialClassUI> table;


        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            var focusAdventurer = RaidDirector.FocusVenturer;
            var targetTutorial = table[focusAdventurer.Type];

            if (!targetTutorial.IsAlreadyShown)
            {
                targetTutorial.ShowVenturerTutorial();
            }
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            table.CreateTable(GetComponentsInChildren<TutorialClassUI>(true), list => list.VenturerType);
        }
#endif
    }
}
