namespace Common
{
    [System.Serializable]
    public class DefenseValueEntity
    {
        public DefenseValueEntity(DefenseValueEntity reference) 
            : this(reference.Hp, reference.MaxHp, reference.MoveSpeed, reference.Armor, reference.Evade, reference.Resist) {}
        public DefenseValueEntity(float hp, float maxHp, float moveSpeed, float armor, float evade, float resist)
        {
            Hp = hp;
            MaxHp = maxHp;
            MoveSpeed = moveSpeed;
            Armor = armor;
            Evade = evade;
            Resist = resist;
        }

        public float Hp { get; set; }
        public float MaxHp { get; set; }
        public float MoveSpeed { get; set; }
        public float Armor { get; set; }
        public float Evade { get; set; }
        public float Resist { get; set; }
        // public float CriticalRate { get; set; } 치명타 데미지 비율
        // public float CriticalChance { get; set; } 치명타 받을 확률

        public static DefenseValueEntity operator +(DefenseValueEntity valueEntityLeft, DefenseValueEntity valueEntityRight)
        {
            return new DefenseValueEntity
            (
                valueEntityLeft.Hp + valueEntityRight.Hp,
                valueEntityLeft.MaxHp + valueEntityRight.MaxHp,
                valueEntityLeft.MoveSpeed + valueEntityRight.MoveSpeed,
                valueEntityLeft.Armor + valueEntityRight.Armor,
                valueEntityLeft.Evade + valueEntityRight.Evade,
                valueEntityLeft.Resist + valueEntityRight.Resist
            );
        }
    }
}
