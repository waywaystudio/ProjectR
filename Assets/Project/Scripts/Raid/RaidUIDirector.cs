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
        [SerializeField] private DamageTextPool damageTextPool;

        public StageSetter StageInitializer => stageInitializer;
        

        public void Initialize()
        {
            actionFrame.gameObject.SetActive(true);
            
            bossFrame.Initialize();
            actionFrame.Initialize();
            damageTextPool.Initialize(RaidDirector.AdventurerList);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            stageInitializer ??= GetComponentInChildren<StageSetter>();
            bossFrame        ??= GetComponentInChildren<BossFrame>();
            actionFrame      ??= GetComponentInChildren<ActionFrame>();
            damageTextPool   ??= GetComponentInChildren<DamageTextPool>();
        }
#endif
    }
}
