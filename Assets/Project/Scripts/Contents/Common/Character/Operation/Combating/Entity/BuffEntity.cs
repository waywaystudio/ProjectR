namespace Common.Character.Operation.Combating.Entity
{
    public class BuffEntity : StatusEffectEntity
    {
        private void Reset()
        {
            flag = EntityType.Buff;
            SetEntity();
        }
    }
}
