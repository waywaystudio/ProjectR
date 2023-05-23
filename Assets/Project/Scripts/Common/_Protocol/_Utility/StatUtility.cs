namespace Common
{
    public static class StatUtility
    {
        public static string ToStatUIValue(this float value, StatType type) => type switch
        {
            StatType.Power          => value.ToString("0"),
            StatType.Health         => value.ToString("0"),
            StatType.CriticalChance => $"{value:F1}",
            StatType.CriticalDamage => $"{200 + value:F1}",
            StatType.Haste          => $"{value:F1}",
            StatType.Mastery        => $"{value:F1}",
            StatType.Retention      => $"{value:F1}",
            StatType.Armor          => value.ToString("0"),
            StatType.MoveSpeed      => value.ToString("0"),
            StatType.MaxHp          => value.ToString("0"),
            StatType.MaxResource    => value.ToString("0"),
            StatType.MinDamage      => value.ToString("0"),
            StatType.MaxDamage      => value.ToString("0"),
            _                       => "None",
        };
    }
}
