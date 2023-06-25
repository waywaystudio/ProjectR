using System;
using Common.Animation;

namespace Character.Venturers
{
    public class VenturerAnimationModel : AnimationModel
    {
        public override void Idle() => PlayLoop("Idle");
        public override void Run() => PlayLoop("Run");
        public override void Dead(Action callback = null) => PlayOnce("death", 0f, callback);
        public override void Stun() => PlayLoop("Stun");
        public override void Hit() => PlayLoop("Hit");
    }
}
