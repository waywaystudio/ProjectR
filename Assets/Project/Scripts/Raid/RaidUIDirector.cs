using Raid.UI.BossFrames;
using Raid.UI.BriefingFrames;
using Raid.UI.FloatingTexts;
using Raid.UI.VenturerFrames;
using UnityEngine;

namespace Raid
{
    public class RaidUIDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private Briefing stageInitializer;
        
        // TODO. 추후에 bossFrame 한꺼번에 Initialize하기
        [SerializeField] private BossHpProgress bossHpProgress;
        
        // Venturer Frames
        [SerializeField] private VenturerSkillBar skillBar;

        // Pool
        [SerializeField] private FloatingTextPool floatingTextPool;


        public void Initialize()
        {
            bossHpProgress.Initialize();
            skillBar.Initialize();
        }

        public void Dispose()
        {
            skillBar.Dispose();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            stageInitializer ??= GetComponentInChildren<Briefing>();
            skillBar         ??= GetComponentInChildren<VenturerSkillBar>();
            floatingTextPool ??= GetComponentInChildren<FloatingTextPool>();
        }
#endif
    }
}
