using Common.Animation;

namespace Character.Venturers.Rogue
{
    public class PhantomAnimationModel : AnimationModel
    {
        public override void Idle() => PlayLoop("Idle");
    }
}
