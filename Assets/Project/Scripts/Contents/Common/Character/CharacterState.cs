namespace Common.Character
{
    [System.Flags]
    public enum CharacterState
    {
        None = 1 << 0,
        Idle = 1 << 1,
        Attack = 1 << 2,
        Run = 1 << 3,
        Crouch = 1 << 4,
        All = int.MaxValue
    }
}
