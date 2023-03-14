using UnityEngine;
using UnityEngine.Serialization;

namespace Raid.UI
{
    using ActionFrames;
    using ActionFrames.ActionBars;
    
    public class ActionFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private CastingProgress castingProgress;
        [SerializeField] private StatusEffectBar statusEffectBar;
        [SerializeField] private DynamicValueProcess dynamicValueProcess;
        [SerializeField] private SkillActionBar skillActionBar;
        [SerializeField] private AdventurerBar adventurerBar;
        
        public void Initialize()
        {
            castingProgress.Initialize();
            statusEffectBar.Initialize(RaidDirector.FocusCharacter);
            dynamicValueProcess.Initialize(RaidDirector.FocusCharacter);
            skillActionBar.Initialize(RaidDirector.FocusCharacter);
            adventurerBar.Initialize(RaidDirector.AdventurerList);
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            castingProgress     ??= GetComponentInChildren<CastingProgress>();
            statusEffectBar     ??= GetComponentInChildren<StatusEffectBar>();
            dynamicValueProcess ??= GetComponentInChildren<DynamicValueProcess>();
            skillActionBar            ??= GetComponentInChildren<SkillActionBar>();
            adventurerBar       ??= GetComponentInChildren<AdventurerBar>();
        }
#endif
    }
}
