using UnityEngine;

namespace Character
{
    public static class CharacterUtility
    {
        /// <summary>
        /// Haste Additional Value, for reduce GlobalCoolTime, Casting Time, BuffDuration
        /// </summary>
        /// <returns>usually less than 1.0f value</returns>
        public static float GetHasteValue(float haste) => 100f * (1f / (100 * (1f + haste)));

        /// <summary>
        /// Inversed Haste Additional Value for faster Animation Speed
        /// </summary>
        /// <returns>usually more than 1.0f value</returns>
        public static float GetInverseHasteValue(float haste) => 1f + haste;
        
        

        /// <summary>
        /// Set FlagNumber (ex.128, 256 etc. not index)
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="allyOrEnemy"></param>
        /// <returns></returns>
        public static LayerMask SetLayer(IObjectName provider, string allyOrEnemy)
        {
            var selfLayer = (LayerMask)(1 << provider.Object.layer);
            
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
