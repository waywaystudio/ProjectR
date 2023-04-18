using System;
using UnityEngine;

namespace Common
{
    public interface IActiveSection { ActionTable OnActivated { get; }}
    public interface ICancelSection { ActionTable OnCanceled { get; }}
    public interface ICompleteSection { ActionTable OnCompleted { get; }}
    public interface IEndSection { ActionTable OnEnded { get; }}

    public interface ISequence : IActiveSection, ICancelSection, ICompleteSection, IEndSection
    {
        // ActionTable OnActivated { get; }
        // ActionTable OnCanceled { get; }
        // ActionTable OnCompleted { get; }
        // ActionTable OnEnded { get; }
    }

    public interface IConditionalSequence : ISequence
    {
        ConditionTable Conditions { get; }
    }

    public interface IProjectorSequence : ISequence
    {
        float CastingTime { get; }
        Vector2 SizeVector { get; }
    }

    public static class SequenceExtension
    {
        /// <summary>
        /// main에서 Action을 실행할 때, sub의 ActionTable을 참조한다. 
        /// </summary>
        public static void CombineAsReference(this ISequence main, string key, ISequence sub)
        {
            main.OnActivated.Register(key, sub.OnActivated);
            main.OnCanceled.Register(key, sub.OnCanceled);
            main.OnCompleted.Register(key, sub.OnCompleted);
            main.OnEnded.Register(key, sub.OnEnded);
        }
        
        /// <summary>
        /// 결합 시점에 sub 시퀀스 ActionTable의 Action만 포함한다. 
        /// </summary>
        public static void CombineAsValue(this ISequence main, ISequence sub)
        {
            main.OnActivated.Register(sub.OnActivated);
            main.OnCanceled.Register(sub.OnCanceled);
            main.OnCompleted.Register(sub.OnCompleted);
            main.OnEnded.Register(sub.OnEnded);
        }

        public static void Clear(this ISequence sequence)
        {
            sequence.OnActivated.Clear();
            sequence.OnCanceled.Clear();
            sequence.OnCompleted.Clear();
            sequence.OnEnded.Clear();
        }

        public static ActionTable ConvertSection(this ISequence sequence, SectionType sectionType)
        {
            return sectionType switch
            {
                SectionType.None => throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null),
                SectionType.OnActivated => sequence.OnActivated,
                SectionType.OnCanceled => sequence.OnCanceled,
                SectionType.OnCompleted => sequence.OnCompleted,
                SectionType.OnEnded     => sequence.OnEnded,
                _                       => throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null)
            };
        }
    }
}