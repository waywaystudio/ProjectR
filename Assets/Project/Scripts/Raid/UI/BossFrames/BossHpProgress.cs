using Common.UI;
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
            
            hpImage.RegisterEvent(mb.Hp, mb.StatTable.MaxHp);
            monsterName.text = mb.Name;
            mb.Hp.AddListener("UI.BossFrames.BossHpProcess", ValueToText);
            
            ValueToText();
        }

        private void ValueToText()
        {
            hpValueText.text = RaidDirector.Boss.Hp.Value.ToString("0");
        }
        

        [Button] public void MinusHp() => RaidDirector.Boss.Hp.Value -= 50f;
        [Button] public void MinusResource() => RaidDirector.Boss.Resource.Value -= 8f;
    }
}
