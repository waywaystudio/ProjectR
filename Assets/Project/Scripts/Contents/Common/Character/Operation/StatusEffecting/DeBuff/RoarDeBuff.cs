using System.Collections;
using UnityEngine;

namespace Common.Character.Operation.StatusEffecting.DeBuff
{
    public class RoarDeBuff : BaseStatusEffect
    {
        private const string Roar = "Roar";
        private WaitForSeconds waitForSeconds;
        
        public override IEnumerator MainAction()
        {
            waitForSeconds = new WaitForSeconds(Duration);

            var cb = TakerInfo.Taker.GetComponent<CharacterBehaviour>();
            cb.ArmorTable.RegisterMultiType(Roar, () => 0.85f);

            yield return waitForSeconds;
            
            cb.ArmorTable.UnregisterMultiType(Roar);
            Callback?.Invoke();
        }
    }
}
