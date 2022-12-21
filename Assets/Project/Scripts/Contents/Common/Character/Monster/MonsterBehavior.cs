using Core;
using UnityEngine;

namespace Common.Character
{
    public class MonsterBehavior : CharacterBehaviour
    {
        protected override void Start()
        {
            base.Start();
            
            MoveSpeedTable.Register("MB", 20f);
        }

        protected new void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
        
            // ReSharper disable once Unity.PerformanceCriticalCodeCameraMain
            // ReSharper disable once PossibleNullReferenceException
            #region TEST FUNCTION

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;

            Run(hit.point);

            #endregion
        }

        private void OnDisable()
        {
            MoveSpeedTable.Unregister("MB");
        }

        public override GameObject Object => gameObject;
    }
}
