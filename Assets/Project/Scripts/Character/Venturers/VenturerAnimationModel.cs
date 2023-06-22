using System;
using System.Collections.Generic;
using Common.Animation;
using UnityEngine;

namespace Character.Venturers
{
    public class VenturerAnimationModel : AnimationModel
    {
        [SerializeField] private List<string> keyList;
        
        public override void Idle() => PlayLoop("Idle");
        public override void Run() => PlayLoop("Run");
        public override void Dead(Action callback = null) => PlayOnce("death", 0f, callback);
        public override void Stun() => PlayLoop("Stun");
        public override void Hit() => PlayLoop("Hit");


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            keyList.Clear();
            keyList.AddRange(new []
            {
                "Attack1",
                "Attack2",
                "Attack 1 DUELIST",
                "Attack 2 DUELIST",
                "Attack 3 DUELIST",
                "Idle",
                "Idle ARCHER",
                "Walk",
                "Run",
                "Run ARCHER",
                "Run DUELIST",
                "Jump",
                "Jump1",
                "Jump1 ARCHER",
                "Jump2",
                "Jump3",
                "Jump3 ARCHER",
                "Buff",
                "Hurt",
                "Defence",
                "Death",
                "Shoot1",
                "Shoot2",
                "Shoot3",
                "Cast1",
                "Cast2",
                "Cast3",
                "Fly",
            });
        }
#endif
    }
}
