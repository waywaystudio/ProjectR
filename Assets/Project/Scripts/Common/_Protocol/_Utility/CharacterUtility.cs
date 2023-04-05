using Common.Systems;
using UnityEngine;

namespace Common
{
    public static class CharacterUtility
    {
        /// <summary>
        /// Set FlagNumber (ex.128, 256 etc. not index)
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="allyOrEnemy"></param>
        /// <returns></returns>
        public static LayerMask SetLayer(IObjectName provider, string allyOrEnemy)
        {
            var selfLayer = (LayerMask)(1 << provider.gameObject.layer);
            
            return allyOrEnemy is "ally"
                ? selfLayer
                : selfLayer.GetEnemyLayerMask();
        }

        public static LayerMask IndexToLayerValue(GameObject go) => IndexToLayerValue(go.layer);
        public static LayerMask IndexToLayerValue(int index)
        {
            if (index > 20)
            {
                Debug.LogError($"value must less then 20. input:{index}");
                return 0;
            }

            return 1 << index;
        }
    }
}
