using Raid.UI.FloatingTexts;
using Raid.UI.StageInitializer;
using UnityEngine;

namespace Raid
{
    using UI;

    public class RaidUIDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private StageSetter stageInitializer;
        [SerializeField] private BossFrame bossFrame;
        [SerializeField] private ActionFrame actionFrame;

        // Pool
        [SerializeField] private FloatingTextPool floatingTextPool;

        public StageSetter StageInitializer => stageInitializer;
        

        public void Initialize()
        {
            actionFrame.gameObject.SetActive(true);
            
            bossFrame.Initialize();
            actionFrame.Initialize();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            stageInitializer ??= GetComponentInChildren<StageSetter>();
            bossFrame        ??= GetComponentInChildren<BossFrame>();
            actionFrame      ??= GetComponentInChildren<ActionFrame>();
            floatingTextPool   ??= GetComponentInChildren<FloatingTextPool>();
        }
#endif
    }
}
