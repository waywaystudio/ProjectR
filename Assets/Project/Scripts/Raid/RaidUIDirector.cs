using System.Collections.Generic;
using Raid.UI.FloatingTexts;
using Raid.UI.PartyFrames;
using Raid.UI.VenturerFrames;
using Raid.UI.VillainFrames;
using UnityEngine;

namespace Raid
{
    public class RaidUIDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private CombatTextPool combatTextPool;
        [SerializeField] private VillainHealthBar villainHealthBar;
        [SerializeField] private VillainStatusEffectUI villainStatusEffectBar;
        [SerializeField] private VenturerSkillBar skillBar;
        [SerializeField] private List<UnitFrame> unitFrames;

        public void Initialize()
        {
            /* Villain */
            villainHealthBar.Initialize();
            villainStatusEffectBar.Initialize();
            
            /* Venturers */
            skillBar.Initialize();
            
            /* UnitFrames */ 
            unitFrames.ForEach(frame => frame.gameObject.SetActive(false));
            
            RaidDirector.VenturerList.ForEach((venturer, index) =>
            {
                if (unitFrames.Count <= index) return;
                
                unitFrames[index].gameObject.SetActive(true);
                unitFrames[index].Initialize(venturer);
            });
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            villainHealthBar       ??= GetComponentInChildren<VillainHealthBar>();
            villainStatusEffectBar ??= GetComponentInChildren<VillainStatusEffectUI>();
            skillBar               ??= GetComponentInChildren<VenturerSkillBar>();
            combatTextPool         ??= GetComponentInChildren<CombatTextPool>();

            GetComponentsInChildren(true, unitFrames);
        }
#endif
    }
}
