using System.Collections.Generic;

namespace Core
{
    public interface ICombatBehaviour : IGlobalCoolTimer
    {
        // + Observable<float> GlobalCoolTimer { get; }
        // + float GlobalCoolTime { get; }
        
        List<ISkillInfo> SkillInfoList { get; }
    }

    public interface IGlobalCoolTimer
    {
        Observable<float> GlobalRemainTime { get; }
        float GlobalCoolTime { get; }
    }
}