using System.Collections.Generic;

namespace Core
{
    public interface ISkillBehaviour : IGlobalCoolTimer
    {
        // + Observable<float> GlobalCoolTimer { get; }
        // + float GlobalCoolTime { get; }
        
        List<ISkillInfo> SkillInfoList { get; }
    }

    public interface IGlobalCoolTimer
    {
        OldObservable<float> GlobalRemainTime { get; }
        float GlobalCoolTime { get; }
    }
}