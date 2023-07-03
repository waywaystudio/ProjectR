using System;

[Serializable] 
public class Sequencer
{
    public ConditionTable Condition { get; } = new();
    public WaitTrigger CompleteTrigger { get; set; }
    public Table<SectionType, ActionTable> Table { get; set; } = new();
    public ActionTable this[SectionType key]
    {
        get
        {
            if (!Table.ContainsKey(key))
            {
                Table.Add(key, new ActionTable());
            }

            return Table[key];
        }
    }
   
    public virtual void Clear()
    {
        Condition.Clear();
        CompleteTrigger?.Dispose();
        Table.Clear();
    }
}

[Serializable] 
public class Sequencer<T> : Sequencer
{
    public ActionTable<T> ActiveParamAction { get; } = new();

    public override void Clear()
    {
        base.Clear();
        
        ActiveParamAction.Clear();
    }
}
