using Character;
using MainGame.UI.ImageUtility;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Raid.UI.BossFrames
{
    public class BossHpProgress : MonoBehaviour
    {
        [SerializeField] private ImageFiller hpImage;
        [SerializeField] private TextMeshProUGUI monsterName;
        [SerializeField] private TextMeshProUGUI hpValueText;


        public void Initialize()
        {
            var mb = RaidDirector.Boss;
            
            hpImage.Register(mb.DynamicStatEntry.Hp, mb.StatTable.MaxHp);
            monsterName.text = mb.Name;
            mb.DynamicStatEntry.Hp.Register("UI.BossFrames.BossHpProcess", ValueToText);
            
            ValueToText();
        }

        private void ValueToText()
        {
            hpValueText.text = RaidDirector.Boss.DynamicStatEntry.Hp.Value.ToString("0");
        }

        [Button] public void MinusHp() => RaidDirector.Boss.DynamicStatEntry.Hp.Value -= 50f;
        [Button] public void MinusResource() => RaidDirector.Boss.DynamicStatEntry.Resource.Value -= 8f;
    }
}
