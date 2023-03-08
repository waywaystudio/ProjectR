using Core;
using MainGame.UI.Tooltips;
using UnityEngine;

namespace Raid
{
    using UI;

    public class RaidUIDirector : MonoBehaviour, IEditable
    {
        [SerializeField] private BossFrame bossFrame;
        [SerializeField] private ActionFrame actionFrame;

        // Pool
        [SerializeField] private DamageTextPool damageTextPool;
        [SerializeField] private TooltipPool tooltipPool;

        public TooltipPool TooltipPool => tooltipPool;

        // [SerializeField] private List<PartyUnitFrame> partyFrameList;
        // public List<PartyUnitFrame> PartyFrameList => partyFrameList;
        public void Initialize()
        {
            bossFrame.Initialize();
            actionFrame.Initialize();
            damageTextPool.Initialize(RaidDirector.AdventurerList);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            bossFrame      ??= GetComponentInChildren<BossFrame>();
            actionFrame    ??= GetComponentInChildren<ActionFrame>();
            damageTextPool ??= GetComponentInChildren<DamageTextPool>();
            tooltipPool    ??= GetComponentInChildren<TooltipPool>();
        }
#endif
    }
}
