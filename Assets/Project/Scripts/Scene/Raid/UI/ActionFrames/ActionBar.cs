using Core;
using UnityEngine;

namespace Raid.UI.ActionFrames
{
    using ActionBars;
    
    public class ActionBar : MonoBehaviour, IEditable
    {
        [SerializeField] private AdventurerBar adventurerBars;
        [SerializeField] private CharacterSkillBar characterSkillBar;


        public void Initialize()
        {
            characterSkillBar.Initialize();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            characterSkillBar ??= GetComponentInChildren<CharacterSkillBar>();
        }
#endif
    }
}
