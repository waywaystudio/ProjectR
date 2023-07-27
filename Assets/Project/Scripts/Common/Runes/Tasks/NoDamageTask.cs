namespace Common.Runes.Tasks
{
    public class NoDamageTask : TaskRune
    {
        public override TaskRuneType RuneType => TaskRuneType.NoDamage;
        public override string Description => "Do Not Take any damage in a single battle";

        public override void ActiveTask()
        {
            Progress.Value = 0f;
            Max      = 1f;
        }

        public override void Accomplish()
        {
            if (Tasker.BattleLog.TotalDamaged == 0f)
            {
                Progress.Value = 1f;
                IsSuccess      = true;
            }
        }

        public override void Defeat()
        {
            Progress.Value = 0f;
        }
    }
}
