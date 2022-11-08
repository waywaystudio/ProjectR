using GoogleSheet.Core.Type;

namespace Adventurer
{
    [UGS(typeof(Role))]
    public enum Role
    {
        None = 0,
        Tank,
        Melee,
        Range,
        Healer,
        End = int.MaxValue
    }
}
