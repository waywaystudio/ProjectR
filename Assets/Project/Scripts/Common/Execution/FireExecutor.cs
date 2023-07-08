using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Execution
{
    [Serializable]
    public class FireExecutor
    {
        [SerializeField] private List<FireExecution> fireExecutionList;

        public void Initialize(CombatSequence sequence, IActionSender sender)
        {
            fireExecutionList.ForEach(exe => exe.Initialize(sender));
            
            var builder = new CombatSequenceBuilder(sequence);
            var mainFire = fireExecutionList.FindAll(exe => exe.Group == ExecuteGroup.Main);
            var subFire = fireExecutionList.FindAll(exe => exe.Group  == ExecuteGroup.Sub);
            
            if (mainFire.Count > 0) builder.AddFire("FireExecution", position => mainFire.ForEach(exe => exe.Fire(position)));
            if (subFire.Count  > 0) builder.AddSubFire("SubFireExecution", position => subFire.ForEach(exe => exe.Fire(position)));
        }

        
        
#if UNITY_EDITOR
        public void GetExecutionInEditor(Transform parent)
        {
            parent.GetComponentsInChildren(fireExecutionList);
        }
#endif
    }
}
