using System.Collections.Generic;
using Raid.UI.StageInitializer;
using UnityEngine;

namespace Raid.UI.BriefingFrames
{
    public class Briefing : MonoBehaviour, IEditable
    {
        [SerializeField] private List<AdventurerCard> adventurerCardList;

        public void LetsStart()
        {
            RaidDirector.Initialize();
            
            gameObject.SetActive(false);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(false, adventurerCardList);
        }
#endif
    }
}
