namespace Common
{
    public static class StatUtility
    {
        public static string ToStatUIValue(this float value, StatType type) => type switch
        {
            StatType.Power          => value.ToString("0"),
            StatType.Health         => value.ToString("0"),
            StatType.CriticalChance => $"{value:P1}",
            StatType.CriticalDamage => $"{200 + value:F1}",
            StatType.Haste          => $"{value:P1}",
            StatType.Mastery        => $"{value:P1}",
            StatType.Retention      => $"{value:P1}",
            StatType.Armor          => value.ToString("0"),
            StatType.MoveSpeed      => value.ToString("0"),
            StatType.MaxResource    => value.ToString("0"),
            StatType.MinDamage      => value.ToString("0"),
            StatType.MaxDamage      => value.ToString("0"),
            _                       => "None",
        };
    }
}
