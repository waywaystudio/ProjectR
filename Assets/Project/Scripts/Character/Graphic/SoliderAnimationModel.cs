namespace Character.Graphic
{
    public class SoliderAnimationModel : AnimationModel
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            Finder.TryGetObject("SoldierAnimationModelData", out modelData);
        }
#endif
    }
}
