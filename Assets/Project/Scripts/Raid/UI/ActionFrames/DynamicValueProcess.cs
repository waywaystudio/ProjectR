using Character;
using Sirenix.OdinInspector;
using UI.ImageUtility;
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

        // TODO. 현재 체력이 깎이지 않아 테스트 함수.
        [Button] public void MinusHp() => focusedAdventurer.DynamicStatEntry.Hp.Value -= 50f;
        [Button] public void MinusResource() => focusedAdventurer.DynamicStatEntry.Resource.Value -= 8f;
    }
}
