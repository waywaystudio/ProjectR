using Common;

namespace Camps.Inventories
{
    public class GrowMaterialInventory : Inventory<GrowMaterialType>
    {
        protected override string SerializeKey => "GrowMaterialSerializeKey";


#if UNITY_EDITOR
        public void AddAll100Material()
        {
            GrowMaterialType.None.Iterator(viceType =>
            {
                if (viceType == GrowMaterialType.None) return;
                
                Add(viceType, 100);
            });
        }
#endif
        
    }
}
