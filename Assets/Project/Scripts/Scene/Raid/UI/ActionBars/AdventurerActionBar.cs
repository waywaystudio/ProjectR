using Character;
using MainGame.UI.ActionBars;
using UnityEngine;

namespace Raid.UI.ActionBars
{
    public class AdventurerActionBar : ActionBar
    {
        [SerializeField] private GameObject adventurerPrefab;

        private Adventurer adventurer;
    }
}
