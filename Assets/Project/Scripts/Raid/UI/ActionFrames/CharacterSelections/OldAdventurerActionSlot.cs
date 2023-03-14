using Adventurers;
using Character.Adventurers;
using Manager;
using TMPro;
using UnityEngine;

namespace Raid.UI.ActionFrames.ActionBars.AdventurerBars
{
    public class OldAdventurerActionSlot : MonoBehaviour
    {
        [SerializeField] private OldAdventurerActionIcon adventurerAction;
        [SerializeField] private TextMeshProUGUI hotKey;
        [SerializeField] private BindingCode bindingCode;

        private Adventurer focusedAdventurer;
        private string HotKey =>
            bindingCode switch
            {
                BindingCode.Keyboard1 => "1",
                BindingCode.Keyboard2 => "2",
                BindingCode.Keyboard3 => "3",
                BindingCode.Keyboard4 => "4",
                _ => "-",
            };
        

        public void Initialize(Adventurer adventurer)
        {
            focusedAdventurer = adventurer;
            adventurerAction.Initialize(adventurer);

            hotKey.text = HotKey;
            MainManager.Input.TryGetAction(bindingCode, out var inputAction);

            inputAction.started += adventurerAction.StartAction;
        }
    }
}