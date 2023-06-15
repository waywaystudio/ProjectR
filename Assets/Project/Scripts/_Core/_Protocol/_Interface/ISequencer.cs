using Sequences;

public interface ISequencer
{
    ConditionTable Condition { get; }
    OldSection ActiveSection { get; }
    OldSection CancelSection { get; }
    OldSection CompleteSection { get; }
    OldSection EndSection { get; }
}

public interface ISequencer<T> : ISequencer
{
    OldSection<T> ActiveParamSection { get; }
    
    // + ConditionTable Condition { get; }
    // + Section Activation { get; }
    // + Section Cancellation { get; }
    // + Section Complete { get; }
    // + Section End { get; }
}
