using Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid.UI.DynamicValueProcesses
{
    public class DynamicValueProcess : MonoBehaviour
    {
        // TODO. TEMP
        [SerializeField] private AdventurerBehaviour ab;
        //
        [SerializeField] private ImageFiller hpImage;
        [SerializeField] private ImageFiller resourceImage;
        
        private AdventurerBehaviour focusedAdventurer;
        

        public void Initialize(AdventurerBehaviour ab)
        {
            focusedAdventurer = this.ab = ab;
            
            // TODO. MaxHp가 const형태로 들어가게 된다.
            hpImage.Register(ab.DynamicStatEntry.Hp, ab.StatTable.MaxHp);
            resourceImage.Register(ab.DynamicStatEntry.Resource, ab.StatTable.MaxResource);
        }

        private void Start()
        {
            Initialize(ab);
        }

        // TODO. 현재 체력이 깎이지 않아 테스트 함수.
        [Button] public void MinusHp() => focusedAdventurer.DynamicStatEntry.Hp.Value -= 10f;
        [Button] public void MinusResource() => focusedAdventurer.DynamicStatEntry.Resource.Value -= 4f;
    }
}
