using TMPro;
using UnityEngine;

namespace Common.UI
{
    public class StatInfoUI : MonoBehaviour, IEditable
    {
        [SerializeField] private StatType statType;
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private TextMeshProUGUI valueText;

        public StatType StatType => statType;


        public void SetValue(string value)
        {
            valueText.text = value;
        }
        
        public void SetValue(Stat stat)
        {
            valueText.text = ConvertStatToUIText(stat);
        }
        
        
        private static string ConvertStatToUIText(Stat stat) => stat.StatType switch
        {
            StatType.Power               => stat.Value.ToString("0"),
            StatType.Health              => stat.Value.ToString("0"),
            StatType.CriticalChance      => $"{stat.Value:F1}%",
            StatType.CriticalDamage      => $"{200 + stat.Value:F1}%",
            StatType.Haste               => $"{stat.Value:F1}%",
            StatType.Armor               => stat.Value.ToString("0"),
            StatType.MoveSpeed           => stat.Value.ToString("0"),
            StatType.MaxHp               => stat.Value.ToString("0"),
            StatType.MaxResource         => stat.Value.ToString("0"),
            StatType.MinDamage           => stat.Value.ToString("0"),
            StatType.MaxDamage           => stat.Value.ToString("0"),
            StatType.ExtraPower          => stat.Value.ToString("0"),
            StatType.ExtraCriticalChance => $"{stat.Value:F1}%",
            StatType.ExtraCriticalDamage => $"{200 + stat.Value:F1}%",
            _                            => "UnDefined Format",
        };


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            if (gameObject.name != statType.ToString()) 
                gameObject.name = statType.ToString();
            
            labelText      = transform.Find("Label").GetComponent<TextMeshProUGUI>();
            valueText      = transform.Find("Value").GetComponent<TextMeshProUGUI>();
            labelText.text = statType.ToString();
            valueText.text = "##.#";
        }
#endif
    }
}
