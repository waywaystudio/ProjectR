using Raid.UI.BriefingFrames;
using Raid.UI.FloatingTexts;
using UnityEngine;

namespace Raid
{
    public class RaidUIDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private Briefing stageInitializer;

        // Pool
        [SerializeField] private FloatingTextPool floatingTextPool;


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            stageInitializer ??= GetComponentInChildren<Briefing>();
            floatingTextPool   ??= GetComponentInChildren<FloatingTextPool>();
        }
#endif
    }
}
