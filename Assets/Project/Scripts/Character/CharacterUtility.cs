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
    }
}
