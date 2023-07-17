using Common.UI;
using TMPro;
using UnityEngine;

namespace Raid.UI.VillainFrames
{
    public class VillainHealthBar : MonoBehaviour, IEditable
    {
        [SerializeField] private ImageFiller hpImage;
        [SerializeField] private TextMeshProUGUI hpValueText;
        [SerializeField] private TextMeshProUGUI monsterName;


        public void Initialize()
        {
            var vb = RaidDirector.Boss;
            
            hpImage.RegisterEvent(vb.Hp, vb.StatTable.MaxHp);
            monsterName.text = vb.Name;
            vb.Hp.AddListener("ValueToTextUI", ValueToTextUI);
            
            ValueToTextUI();
        }

        private void ValueToTextUI()
        {
            hpValueText.text = RaidDirector.Boss.Hp.Value.ToString("0");
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            hpImage     = transform.Find("Fill").GetComponent<ImageFiller>();
            hpValueText = transform.Find("ValueText").GetComponent<TextMeshProUGUI>();
            monsterName = transform.Find("VillainName").Find("Name").GetComponent<TextMeshProUGUI>();
            
        }
#endif
    }
}
