using Common.Animation;

namespace Animations
{
    public class SoliderAnimationModel : AnimationModel
    {
        public override void Stun() => PlayLoop("hit");
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            Finder.TryGetObject("SoldierAnimationModelData", out modelData);
        }
#endif
    }
}
