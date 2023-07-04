using Common.Animation;

namespace Character.Villains
{
    public class VillainAnimationModel : AnimationModel
    {
        public override void Hit() => PlayLoop("fall");
        public override void Run(float animationSpeed) => PlayLoop("run", 1f);
    }
}
