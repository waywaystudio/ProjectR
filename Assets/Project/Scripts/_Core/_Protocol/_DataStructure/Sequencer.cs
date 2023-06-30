using System;

[Serializable] public class Sequencer
{
    public ConditionTable Condition { get; } = new();
    public WaitTrigger CompleteTrigger { get; set; }
    public Table<SectionType, Section> Table { get; set; } = new();
    public ActionTable this[SectionType key]
    {
        get
        {
            if (!Table.ContainsKey(key))
            {
                Table.Add(key, new Section(key));
            }

            return Table[key].ActionTable;
        }
    }
   
    public void Clear()
    {
        Condition.Clear();
        CompleteTrigger?.Dispose();
        
        Table.Clear();
    }
}

[Serializable] public class Sequencer<T> : Sequencer
{
    public ActionTable<T> ActiveParamAction { get; } = new();

    public new void Clear()
    {
        ActiveParamAction.Clear();
        Condition.Clear();
        CompleteTrigger?.Dispose();
        
        Table.Clear();
    }
}
