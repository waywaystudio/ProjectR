using Raid.UI.BossFrames;
using Raid.UI.FloatingTexts;
using Raid.UI.VenturerFrames;
using UnityEngine;

namespace Raid
{
    public class RaidUIDirector : MonoBehaviour, IEditable
    {
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


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            skillBar         ??= GetComponentInChildren<VenturerSkillBar>();
            floatingTextPool ??= GetComponentInChildren<FloatingTextPool>();
        }
#endif
    }
}
