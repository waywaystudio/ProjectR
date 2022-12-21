using System.Collections;
using UnityEngine;

namespace Common.Character.Operation.StatusEffect.DeBuff
{
    public class RoarDeBuff : BaseStatusEffect
    {
        private const string Roar = "Roar";
        private WaitForSeconds waitForSeconds;
        private CharacterBehaviour cb;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            cb ??= TakerInfo.Object.GetComponent<CharacterBehaviour>();
            cb.ArmorMultiTable.Register(Roar, () => 0.85f);

            yield return waitForSeconds;
            
            cb.ArmorMultiTable.Unregister(Roar);
            Callback?.Invoke();
        }
    }
}
