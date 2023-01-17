using Core;
using UnityEngine;

namespace Character.Combat
{
    public class ResourceModule : CombatModule, IReady
    {
        [SerializeField] private float obtain;
        
        public ICombatProvider Provider => CombatObject.Provider;

        public bool IsReady => obtain switch
        {
            < 0 => Provider.DynamicStatEntry.Resource.Value >= obtain * -1f,
            _ => true,
        };
        

        protected override void Awake()
        {
            base.Awake();
            
            CombatObject.ReadyCheckList.Add(this);
            CombatObject.OnCompleted.Register(InstanceID, () => Provider.DynamicStatEntry.Resource.Value += obtain);
        }

#if UNITY_EDITOR
        public void SetUpValue(float value)
        {
            Flag   = ModuleType.Resource;
            obtain = value;
        }
#endif
    }
}
