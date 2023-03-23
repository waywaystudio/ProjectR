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
            var adventurerEntry = new List<DataIndex>();
            
            adventurerCardList.ForEach(card =>
            {
                if (card.TargetAdventurerIndex != DataIndex.None)
                    adventurerEntry.Add(card.TargetAdventurerIndex);
            });

            RaidDirector.Initialize(adventurerEntry);
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
