using Common.Animation;

namespace Character.Villains
{
    public class VillainAnimationModel : AnimationModel
    {
        public override void Hit() => PlayLoop("fall");
    }
}
