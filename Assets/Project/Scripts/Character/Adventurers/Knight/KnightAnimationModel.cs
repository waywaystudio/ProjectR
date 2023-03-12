using Common.Animation;

namespace Animations
{
    public class KnightAnimationModel : AnimationModel
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            Finder.TryGetObject("KnightAnimationModelData", out modelData);
        }
#endif
    }
}
