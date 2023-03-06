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
        [SerializeField] private AdventurerBar adventurerBar;

        public void Initialize()
        {
            castingProgress.Initialize();
            statusEffectBar.Initialize(RaidDirector.FocusCharacter);
            dynamicValueProcess.Initialize(RaidDirector.FocusCharacter);
            actionBar.Initialize(RaidDirector.FocusCharacter);
            adventurerBar.Initialize(RaidDirector.AdventurerList);
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            castingProgress     ??= GetComponentInChildren<CastingProgress>();
            statusEffectBar     ??= GetComponentInChildren<StatusEffectBar>();
            dynamicValueProcess ??= GetComponentInChildren<DynamicValueProcess>();
            actionBar           ??= GetComponentInChildren<ActionBar>();
            adventurerBar       ??= GetComponentInChildren<AdventurerBar>();
        }
#endif
    }
}
