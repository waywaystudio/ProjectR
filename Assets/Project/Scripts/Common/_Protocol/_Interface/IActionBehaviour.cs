namespace Common
{
    public interface IActionBehaviour
    {
        ActionMask BehaviourMask { get; }
        
        void Cancel();

        // 다른 행동을 취소할 때, Mask Matrix외에 하는 정적행동을 인터페이스에 추가해봤다.
        // 뭔지 잘 모르겠으면 아래 함수를 삭제하고, 각 ActionBehaviour 에서 아래 형태로 구현하자.
        // if (cb.CurrentBehaviour is not null && cb.CurrentBehaviour.BehaviourMask != BehaviourMask) 
        // cb.CurrentBehaviour.Cancel();
        void TryToOverride(IActionBehaviour targetActionBehaviour)
        {
            if (BehaviourMask != ActionMask.None && BehaviourMask != targetActionBehaviour.BehaviourMask) 
                Cancel();
        }
    }
}
