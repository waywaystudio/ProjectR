using Common.Animation;

namespace Character.Venturers.Rogue
{
    public class AssassinAnimationModel : AnimationModel
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            Finder.TryGetObject("AssassinAnimationModelData", out modelData);
        }
#endif
    }
}
