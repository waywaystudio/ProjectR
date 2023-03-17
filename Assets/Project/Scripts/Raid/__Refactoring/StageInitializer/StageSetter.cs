using System.Collections.Generic;
using UnityEngine;

namespace Raid.UI.StageInitializer
{
    public class StageSetter : MonoBehaviour, IEditable
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
