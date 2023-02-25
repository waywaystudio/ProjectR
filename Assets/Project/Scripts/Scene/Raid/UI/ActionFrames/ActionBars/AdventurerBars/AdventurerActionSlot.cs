using Character;
using Core;
using TMPro;
using UnityEngine;

namespace Raid.UI.ActionFrames.ActionBars.AdventurerBars
{
    public class AdventurerActionSlot : MonoBehaviour
    {
        [SerializeField] private AdventurerActionIcon adventurerAction;
        [SerializeField] private TextMeshProUGUI hotKey;
        [SerializeField] private BindingCode bindingCode;

        private string HotKey =>
            bindingCode switch
            {
                BindingCode.Keyboard1 => "1",
                BindingCode.Keyboard2 => "2",
                BindingCode.Keyboard3 => "3",
                BindingCode.Keyboard4 => "4",
                _ => "-",
            };
        

        public void Initialize(AdventurerBehaviour adventurer)
        {
            adventurerAction.Initialize(adventurer);

            hotKey.text = HotKey;
            MainGame.MainManager.Input.TryGetAction(bindingCode, out var inputAction);

            inputAction.started += adventurerAction.StartAction;
        }
    }
}
