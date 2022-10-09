## GameEvent : ScriptableObject   
 * 스크립터블 오브젝트 생성기를 통해서 게임이벤트를 만듭니다.
 * 적당한 이름을 작성하면 끝입니다.
   
## GameEventListener : MonoBehaviour    
  
* 게임오브젝트에 **GameEventListener Component**를 추가 합니다.   
* targetEventList에 구독 하고싶은 게임이벤트를 할당합니다. 
    * 복수의 게임 이벤트를 할당할 수 있습니다.
* Priority 를 통해서 함수의 실행 순서를 조절할 수 있습니다.
  * 값이 작을수록 먼저 실행됩니다.
  * 중복 값이 있어도 괜찮습니다. 먼저 등록한게 먼저 실행됩니다.
* Response는 유니티이벤트 입니다. 실행될 함수를 할당해주세요.
* 다른 Hierarchy의 게임오브젝트를 받을 수 있습니다.    
* 그러나 **Listener가 붙어있는 게임오브젝트 내에서만 할당**하시기를 권장합니다.

- - - 
## **추가**
* **Generic**형태를 제공합니다.
* 하지만 GameEvent와 GameEventListener스크립트를 개별 파일로 작성해주셔야 합니다.
  * 대표적인 타입은 만들어 두었습니다.
* 아래는 decimal 타입 하나를 받는 게임이벤트를 추가하는 상황입니다.

### GameEventDecimal.cs
```C#
namespace Wayway.Engine.Events
{
    public class GameEventDecimal : GameEvent<decimal> {}
}
```
* GameEventDecimal.cs 파일을 추가해주세요.

### GameEventDecimalListener.cs
```C#
namespace Wayway.Engine.Events
{
    public class GameEventDecimalListener : GameEventListener<decimal> {}
}
```
* GameEventDecimalListener.cs 파일을 추가 해 주세요.

- - - 
## **종류**
* 현재 구현되어 있는 데이타 타입은 다음과 같습니다.
* **non-param** Type
```C#
public class Main : MonoBehaviour
{
    public GameEvent GameEvent;
}
```
* int
* long
* float
* double
* string
* UnityEngine.GameObject
```C#
public class Main : MonoBehaviour
{
    public GameEvent<int> GameEventInt;
    public GameEvent<long> GameEventLong;
    public GameEvent<float> GameEventFloat;
    public GameEvent<double> GameEventDouble;
    public GameEvent<string> GameEventString;
    public GameEvent<GameObject> GameEventGameObject;
}
```
- - -
## **특이사항**
* int 타입의 게임이벤트를 사용하기 위해서 변수에 **GameEventInt** 를 사용하지 않고 Generic 타입으로 할 수 있습니다.
* GameEventInt 로 하고 싶으면 해도 됩니다.
```C#
public class Main : MonoBehaviour
{
    public GameEvent<int> GameEventIntType; // GameEventInt SO를 할당할 수 있습니다.
    public GameEventInt GameEventInt;       // GameEventInt SO를 할당할 수 있습니다.
}
```
* **double param** 까지 가능합니다. 
* 필요하면 아래 예시와 같이 만들어 쓰시면 됩니다.
```C#
// GameEventIntAndInt.cs
namespace Wayway.Engine.Events
{
    public class GameEventIntAndInt : GameEvent<int, int> {}
}

// GameEventIntAndIntListener.cs
namespace Wayway.Engine.Events
{
    public class GameEventIntAndIntListener : GameEventListener<int, int> {}
}

```

* 3종류 이상의 parameter가 필요한 상황이오면, Events\Core 폴더에 두 스크립트를 살펴봐주세요.
  * 충분히 만드실 수 있을겁니다.
  * 급한거 아니면 말씀해주세요 제가 넣어 놓을께요.

- - - 
## **패치내역**
* 22.10.01 첫 작성
