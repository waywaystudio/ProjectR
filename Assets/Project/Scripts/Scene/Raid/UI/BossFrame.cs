using Core;
using UnityEngine;

namespace Raid.UI
{
    using BossFrames;
    
    public class BossFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private BossHpProgress hpProgress;
        [SerializeField] private BossStatusEffectBar statusEffectBar;
        [SerializeField] private BossCastingProgress castingProgress;

        public void Initialize()
        {
            hpProgress.Initialize();
            statusEffectBar.Initialize();
            castingProgress.Initialize();
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            hpProgress      ??= GetComponentInChildren<BossHpProgress>();
            statusEffectBar ??= GetComponentInChildren<BossStatusEffectBar>();
            castingProgress ??= GetComponentInChildren<BossCastingProgress>();
        }
#endif
    }
}
