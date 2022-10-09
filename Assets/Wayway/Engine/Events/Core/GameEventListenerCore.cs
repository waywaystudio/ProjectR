using System.Collections.Generic;
using UnityEngine;

namespace Wayway.Engine.Events.Core
{
    public class GameEventListenerCore : MonoBehaviour
    {
        [SerializeField] protected List<GameEventCore> targetEventList;
        [SerializeField] protected int priority = 5;

        protected void OnEnable() => targetEventList.ForEach(x => x.Register(this));
        protected void OnDisable() => targetEventList.ForEach(x => x.UnRegister(this));

        public float Priority => priority;
        public List<GameEventCore> TargetEventList => targetEventList;
    }
}


