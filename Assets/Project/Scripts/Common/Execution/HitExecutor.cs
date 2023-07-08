using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Execution
{
    [Serializable]
    public class HitExecutor 
    {
        [SerializeField] private List<HitExecution> hitExecutionList;

        public void Initialize(CombatSequence sequence, IActionSender sender)
        {
            hitExecutionList.ForEach(exe => exe.Initialize(sender));
            
            var builder = new CombatSequenceBuilder(sequence);
            var mainHit = hitExecutionList.FindAll(exe => exe.Group == ExecuteGroup.Main);
            var subHit = hitExecutionList.FindAll(exe => exe.Group  == ExecuteGroup.Sub);
            
            if (mainHit.Count > 0) builder.AddHit("HitExecution", taker => mainHit.ForEach(exe => exe.Hit(taker)));
            if (subHit.Count  > 0) builder.AddSubHit("SubHitExecution", taker => subHit.ForEach(exe => exe.Hit(taker)));
        }

        
        
#if UNITY_EDITOR
        public void GetExecutionInEditor(Transform parent)
        {
            parent.GetComponentsInChildren(hitExecutionList);
        }
#endif
    }
}
