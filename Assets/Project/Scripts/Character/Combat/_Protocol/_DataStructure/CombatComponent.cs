using Character.Combat.Skill;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Combat
{
    public abstract class CombatComponent : MonoBehaviour
    {
        [SerializeField] protected DataIndex actionCode;
        [SerializeField] protected CombatModuleController moduleController;

        public DataIndex ActionCode => actionCode;
        public ICombatProvider Provider { get; set; }
        [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnCanceled { get; } = new();
        [ShowInInspector] public ActionTable OnHit { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();

        protected abstract void Init();
        
        protected void Awake()
        {
            Provider = GetComponentInParent<ICombatProvider>();
            moduleController.Initialize(this);
            
            Init();
        }
    }
}
