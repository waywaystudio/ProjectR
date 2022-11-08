using System;
using Wayway.Engine.Singleton;

namespace Main.Manager.Control
{
    public class ControlManager : MonoSingleton<ControlManager>
    {
        private IControllable model;

        public void Register(IControllable target) => model = target;
        public void Unregister() => model = null;

        private void FixedUpdate() => model?.Reaction();
    }
}
