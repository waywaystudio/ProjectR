using Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid.UI.BossFrames
{
    public class BossHpProcess : MonoBehaviour
    {
        [SerializeField] private ImageFiller hpImage;
        
        private MonsterBehaviour monsterBehaviour;


        public void Initialize(MonsterBehaviour mb)
        {
            monsterBehaviour = mb;
            
            hpImage.Register(mb.DynamicStatEntry.Hp, mb.StatTable.MaxHp);
        }
        
        // TODO. 현재 체력이 깎이지 않아 테스트 함수.
        [Button] public void MinusHp() => monsterBehaviour.DynamicStatEntry.Hp.Value -= 50f;
        [Button] public void MinusResource() => monsterBehaviour.DynamicStatEntry.Resource.Value -= 8f;
    }
}
