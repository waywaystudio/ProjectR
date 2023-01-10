using System.Collections.Generic;
using System.Linq;
using Character.Combat.StatusEffects;
using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    public class StatusEffectEntity : BaseEntity, ICombatEntity
    {
        [SerializeField] private DataIndex statusEffectCode;
        [SerializeField] private List<StatusEffectObject> statusEffectPool; 
        
        public DataIndex StatusEffectCode { get => statusEffectCode; set => statusEffectCode = value; }
        public IDynamicStatEntry DynamicStatEntry => Provider.DynamicStatEntry;
        public StatTable StatTable => Provider.StatTable;
        
        public override bool IsReady => true;

        public void Effecting(ICombatTaker taker)
        {
            // Pooling.Get use statusEffectCode
            var effect = GetEffect();
            effect.Register(taker);
        }

        public override void Initialize(IActionSender actionSender)
        {
            ActionCode = actionSender.ActionCode;
            Provider   = actionSender.Provider;

            statusEffectPool ??= GetComponentsInChildren<StatusEffectObject>().ToList();
        }


        private StatusEffectObject GetEffect()
        {
            if (statusEffectPool.HasElement()) return statusEffectPool[0];
            
            Debug.LogWarning("Pool Empty!");
            return null;
        }


#if UNITY_EDITOR
        public void SetUpValue(DataIndex effectCode)
        {
            StatusEffectCode =   effectCode;
            Flag             =   EntityType.StatusEffect;
            
            if (statusEffectPool.IsNullOrEmpty())
                statusEffectPool = GetComponentsInChildren<StatusEffectObject>().ToList();
        }
#endif
    }
}

/* Annotation
 StatusEffectEntity에 BaseValue를 바로 계산할 수 없다.
부패와 같이 데미지를 올려주어야 하는 baseValue가 있는 반면에
블러드 처럼 가속에 baseValue를 올려주는 기술이 있는 등, 상태이상에 성격에 따라서 다르기 때문이다.
그러나 Entity 내부에 ICombatProvider를 만들고, 인터페이스를 지울 필요는 또 없다.
이 스킬에 해당하는 기술의 특성이 올라갈 경우, 여기서 올려주는게 맞을 수도 있기 때문이다. */

/* Note
 여기서 근데 뭘 하고 싶은거지... 프로젝타일이랑 다르게 일단 Transform 개념이 필요가 없는데
 코루틴 돌리는거랑, Icon 같은 에셋을 생성할 때 매번 DB에서 가져오는 형태가 싫은 건 알겠는데;
 Prefab이 좀 과한 것 같다. 
 만약 풀링 등 여러가지 이유로 Prefab을 사용한다면 Hierarchy Transform을 이동시키지는 말자.
 딱 하나의 모노로 해결하기에는 Coroutine이 List로 캐쉬화 되어야 한다. Stack도 좋은데...뭐 어쨌든;
 그러니까 정리해보면....
 
 1. Prefab화 계속 진행. Transform이동은 하지 말고...본인 안에서 Coroutine 돌릴 것.
    1-1 Prefab은 스킬 하위에 계속해서 붙어있고, 대상은 별도의 StatusEffects에 List로 해당 프리팹을 Add한다.
    1-2 StatusEffects List다. Dictionary가 아니다. 따라서 같은종류의 스킬이 Add될 수 있다.
 2. Single MonoBehaviour로 기술에 Component 새로 붙히기.
 3. StatusEffectEntity를 확장하기 - 2번구조로 가면 차라리 3번이 좋지 않을까.
 
 * Projectile이랑 비슷하게 가려는건 버리자. 구조를 통일하기에는 오브젝트 메카닉이 꽤 다르다.
 * 1,2,3 모두 어쨌든 캐릭터마다 현재 본인의 버프, 디버프를 관리할 스크립트 하나 필요. UI때문;*/