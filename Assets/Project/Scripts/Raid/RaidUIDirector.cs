using Raid.UI.BriefingFrames;
using Raid.UI.FloatingTexts;
using UnityEngine;

namespace Raid
{
    using UI;

    public class RaidUIDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private Briefing stageInitializer;
        [SerializeField] private BossFrame bossFrame;

        // Pool
        [SerializeField] private FloatingTextPool floatingTextPool;

        public Briefing StageInitializer => stageInitializer;
        

        public void Initialize()
        {
            bossFrame.Initialize();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            stageInitializer ??= GetComponentInChildren<Briefing>();
            bossFrame        ??= GetComponentInChildren<BossFrame>();
            floatingTextPool   ??= GetComponentInChildren<FloatingTextPool>();
        }
#endif
    }
}
