using Core;
using UnityEngine;

namespace Common.Character
{
    public class WillBeChangedMonsterBehavior : CharacterBehaviour
    {
        protected override void Start()
        {
            base.Start();

            StatTable.Register(StatCode.AddMoveSpeed, 11, 20f, true);
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
            StatTable.Unregister(StatCode.AddMoveSpeed, 11);
        }

        public override GameObject Object => gameObject;
    }
}
