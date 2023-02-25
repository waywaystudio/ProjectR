using Core;
using UnityEngine;

namespace Raid.UI
{
    using ActionFrames;
    using ActionFrames.ActionBars;
    using ActionFrames.ActionBars.CharacterSkillBars;
    
    public class ActionFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private CastingProgress castingProgress;
        [SerializeField] private StatusEffectBar statusEffectBar;
        [SerializeField] private DynamicValueProcess dynamicValueProcess;
        [SerializeField] private ActionBar actionBar;
        [SerializeField] private CharacterDashActionSlot dashAction;
        [SerializeField] private AdventurerBar adventurerBar;
        

        public void Initialize()
        {
            castingProgress.Initialize();
            statusEffectBar.Initialize();
            dynamicValueProcess.Initialize();
            actionBar.Initialize();
            dashAction.Initialize();
            adventurerBar.Initialize();
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            castingProgress     ??= GetComponentInChildren<CastingProgress>();
            statusEffectBar     ??= GetComponentInChildren<StatusEffectBar>();
            dynamicValueProcess ??= GetComponentInChildren<DynamicValueProcess>();
            actionBar           ??= GetComponentInChildren<ActionBar>();
            dashAction          ??= GetComponentInChildren<CharacterDashActionSlot>();
            adventurerBar       ??= GetComponentInChildren<AdventurerBar>();
        }
#endif
    }
}
