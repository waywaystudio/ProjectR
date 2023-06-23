using System;

namespace Common.Skills
{
    public class GlobalCoolTimer : CoolTimer
    {
        public float NewCoolTime => coolTime * 
                                    (Retriever is not null 
                                        ? 1f + Retriever.Invoke() 
                                        : 1f);
        
        private Func<float> Retriever { get; set; }

        public void Initialize(Func<float> hasteRetriever)
        {
            Retriever = hasteRetriever;
        }
    }
}
