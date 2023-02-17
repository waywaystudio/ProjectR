using System.Collections.Generic;
using Character;
using Character.StatusEffect;
using Core;
using UnityEngine;

namespace Raid.UI.StatusEffectIconBars
{
    public class StatusEffectIconBar : MonoBehaviour
    {
        // TODO.TEMP
        [SerializeField] private AdventurerBehaviour ab;
        
        [SerializeField] private Transform DeBuffHierarchy;
        [SerializeField] private Transform BuffHierarchy;
        [SerializeField] private List<DeBuffActionSlot> deBuffActionSlotList;
        // [SerializeField] private List<BuffActionSlot> buffActionSlotList;

        // private AdventurerBehaviour ab;


        public void Initialize(AdventurerBehaviour ab)
        {
            // 현재 ab의 디버프 & 버프가 있는 체크 해서 아이콘 등록.
            
            
            // ab가 버프 & 디버프를 받을 때 마다, 아이콘을 등록하는 액션 추가.
            ab.OnTakeStatusEffect.Register("DeBuffUI", OnRegisterDeBuff);
        }

        public void OnRegisterDeBuff(StatusEffectEntity seEntity)
        {
            if (seEntity.IsOverride) return;
            
            foreach (var deBuff in deBuffActionSlotList)
            {
                if (deBuff.isActiveAndEnabled) continue;
            
                deBuff.Register(seEntity);
                break;
            }
        }

        public void RegisterBuff() { }

        public void UnregisterDeBuff(StatusEffectComponent statusEffect)
        {
            foreach (var deBuff in deBuffActionSlotList)
            {
                // if (deBuff.StatusEffect != statusEffect) continue;
                //
                // deBuff.Unregister();
                // return;
            }
        }

        private void Awake()
        {
            GetComponentsInChildren(true, deBuffActionSlotList);
            
            // TODO.TEMP
            Initialize(ab);
        }
        

        public void SetUp()
        {
            GetComponentsInChildren(true, deBuffActionSlotList);
        }
    }
}
