using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Raid.UI.BattleReports
{
    public class RewardPanel : MonoBehaviour, IEditable
    {   
        [SerializeField] private GameObject itemUI;
        [SerializeField] private Transform ContentHierarchy;

        public void Initialize(List<IReward> rewardList)
        {
            if (rewardList.IsNullOrEmpty()) return;
            
            rewardList.ForEach(reward =>
            {
                if (!Verify.IsTrue(Instantiate(itemUI, ContentHierarchy).TryGetComponent(out DropItemUI dropItemUI))) return;
                
                dropItemUI.SetItem(reward);
            });
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            ContentHierarchy = transform.Find("ScrollBox")
                                        .Find("Scroll View")
                                        .Find("Viewport")
                                        .Find("Content");
        }
#endif
    }
}
