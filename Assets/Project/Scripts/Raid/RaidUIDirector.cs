using System;
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
        // TODO.TEMP
        [SerializeField] private GameObject enter;
        
        [SerializeField] private GameObject villainFrames;
        [SerializeField] private GameObject venturerFrames;
        [SerializeField] private GameObject partyFrames;
        
        [SerializeField] private CombatTextPool combatTextPool;
        [SerializeField] private VillainHealthBar villainHealthBar;
        [SerializeField] private VillainStatusEffectUI villainStatusEffectBar;
        [SerializeField] private VenturerSkillBar skillBar;
        [SerializeField] private List<UnitFrame> unitFrames;

        public void Initialize()
        {
            villainFrames.SetActive(true);
            venturerFrames.SetActive(true);
            partyFrames.SetActive(true);
            
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

        private void Awake()
        {
            enter.SetActive(true);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            villainFrames  = transform.Find("VillainFrames").gameObject;
            venturerFrames = transform.Find("VenturerFrames").gameObject;
            partyFrames    = transform.Find("PartyFrames").gameObject;
            
            villainHealthBar       ??= GetComponentInChildren<VillainHealthBar>();
            villainStatusEffectBar ??= GetComponentInChildren<VillainStatusEffectUI>();
            skillBar               ??= GetComponentInChildren<VenturerSkillBar>();
            combatTextPool         ??= GetComponentInChildren<CombatTextPool>();

            GetComponentsInChildren(true, unitFrames);
        }
#endif
    }
}
