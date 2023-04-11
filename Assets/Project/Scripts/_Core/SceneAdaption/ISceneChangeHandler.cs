namespace SceneAdaption
{
    public interface ISceneChangeHandler
    {
        void OnChanging();
        void OnChanged();
    }
}
