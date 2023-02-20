using Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid.UI.DynamicValueProcesses
{
    public class DynamicValueProcess : MonoBehaviour
    {
        [SerializeField] private ImageFiller hpImage;
        [SerializeField] private ImageFiller resourceImage;
        
        private AdventurerBehaviour focusedAdventurer;


        public void Initialize(AdventurerBehaviour ab) => OnFocusChanged(ab);
        public void OnFocusChanged(AdventurerBehaviour ab)
        {
            focusedAdventurer = ab;

            hpImage.Register(ab.DynamicStatEntry.Hp, ab.StatTable.MaxHp);
            resourceImage.Register(ab.DynamicStatEntry.Resource, ab.StatTable.MaxResource);
        }

        // TODO. 현재 체력이 깎이지 않아 테스트 함수.
        [Button] public void MinusHp() => focusedAdventurer.DynamicStatEntry.Hp.Value -= 50f;
        [Button] public void MinusResource() => focusedAdventurer.DynamicStatEntry.Resource.Value -= 8f;
    }
}
