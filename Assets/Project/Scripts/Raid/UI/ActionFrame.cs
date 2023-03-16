using UnityEngine;

namespace Raid.UI
{
    using ActionFrames.ActionBars;
    
    public class ActionFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private AdventurerBar adventurerBar;
        
        public void Initialize()
        {
            // statusEffectBar.Initialize(RaidDirector.FocusCharacter);
            adventurerBar.Initialize(RaidDirector.AdventurerList);
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            adventurerBar       ??= GetComponentInChildren<AdventurerBar>();
        }
#endif
    }
}
