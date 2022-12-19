namespace Common.Character.Operation.Combating.Entity
{
    public class DeBuffEntity : StatusEffectEntity
    {
        private void Reset()
        {
            flag = EntityType.DeBuff;
            SetEntity();
        }
    }
}
