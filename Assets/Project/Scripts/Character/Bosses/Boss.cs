using Common.Characters;
using UnityEngine;

namespace Character.Bosses
{
    public class Boss : CharacterBehaviour
    {
        [SerializeField] private BossPhaseTable phaseTable;

        public BossPhase CurrentPhase { get; set; }

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
    }
}
