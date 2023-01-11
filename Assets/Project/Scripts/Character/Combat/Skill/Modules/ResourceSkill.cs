using Core;
using UnityEngine;

namespace Character.Combat.Skill.Modules
{
    public class ResourceSkill : SkillModule, IResourceModule, IOnCompleted, IReady
    {
        [SerializeField] private float obtain;

        public float Obtain => obtain;
        public ActionTable OnCompleted { get; } = new();
        public bool IsReady => obtain switch
        {
            < 0 => Provider.DynamicStatEntry.Resource.Value >= obtain * -1f,
            _ => true,
        };


        public override void Initialize(IActionSender actionSender)
        {
            base.Initialize(actionSender);
            
            OnCompleted.Register(InstanceID, () => Provider.DynamicStatEntry.Resource.Value += Obtain);
        }
        
        
#if UNITY_EDITOR
        public void SetUpValue(float obtain)
        {
            Flag        = ModuleType.Resource;
            this.obtain = obtain;
        }
#endif
        
    }
}
