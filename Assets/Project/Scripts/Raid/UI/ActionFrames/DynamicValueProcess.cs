using Adventurers;
using Character.Adventurers;
using Common.UI;
using UnityEngine;

namespace Raid.UI.ActionFrames
{
    public class DynamicValueProcess : MonoBehaviour
    {
        [SerializeField] private ImageFiller hpImage;
        [SerializeField] private ImageFiller resourceImage;
        
        private Adventurer focusedAdventurer;


        public void Initialize(Adventurer ab) => OnFocusChanged(ab);
        public void OnFocusChanged(Adventurer ab)
        {
            focusedAdventurer = ab;

            if (ab.IsNullOrEmpty()) return;

            hpImage.Register(ab.DynamicStatEntry.Hp, ab.StatTable.MaxHp);
            resourceImage.Register(ab.DynamicStatEntry.Resource, ab.StatTable.MaxResource);
        }
    }
}
