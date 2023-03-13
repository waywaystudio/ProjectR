using Common.Characters;

namespace Monsters
{
    public class Boss : CharacterBehaviour
    {
        protected void Awake()
        {
            statEntry.Initialize();
        }

        private void Update() { Animating.Flip(transform.forward); }
    }
}
