using System;
using Common.Animation;

namespace Character.Venturers
{
    public class VenturerAnimationModel : AnimationModel
    {
        public override void Idle() => PlayLoop("Idle");
        public override void Run() => PlayLoop("Run");
        public override void Run(float animationSpeed) => PlayLoop("Run", animationSpeed);
        public override void Dead(Action callback = null) => PlayOnce("Death", 1f, callback);
        public override void Stun() => PlayLoop("Stun");
        public override void Hit() => PlayLoop("Hit");
    }
}
