using Core;
using UnityEngine;

namespace Raid
{
    using UI;

    public class RaidUIDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private BossFrame bossFrame;
        [SerializeField] private ActionFrame actionFrame;
        [SerializeField] private DamageTextPool damageTextPool;

        // [SerializeField] private List<PartyUnitFrame> partyFrameList;
        // public List<PartyUnitFrame> PartyFrameList => partyFrameList;
        public void Initialize()
        {
            bossFrame.Initialize();
            actionFrame.Initialize();
            damageTextPool.Initialize();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            bossFrame      ??= GetComponentInChildren<BossFrame>();
            actionFrame    ??= GetComponentInChildren<ActionFrame>();
            damageTextPool ??= GetComponentInChildren<DamageTextPool>();
        }
#endif
    }
}
