using System.Collections.Generic;
using Common.Characters;
using UnityEngine;

namespace Character.Bosses
{
    public class Boss : CharacterBehaviour
    {
        [SerializeField] private BossPhaseTable phaseTable;
        [SerializeField] private BossDropTable dropTable;

        public BossPhase CurrentPhase { get; set; }
        public List<GameObject> DropItemTable => dropTable.DropItemList;

        protected void Awake()
        {
            stats.Initialize();
        }

        protected void Start()
        {
            CurrentPhase = phaseTable.GetStartPhase();
            CurrentPhase.Activate();
        }

        private void Update() { Animating.Flip(transform.forward); }
        
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            phaseTable ??= GetComponent<BossPhaseTable>();
            dropTable  ??= GetComponent<BossDropTable>();
        }
#endif
    }
}

