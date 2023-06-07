using System.Collections.Generic;
using UnityEngine;

namespace Character.Bosses
{
    public class BossDropTable : MonoBehaviour, IEditable
    {
        // TODO. 0.4v 에서 아이템 드랍 메카닉이 정해지지 않아, 랜덤으로 count개의 아이템을 얻는 임시방식을 채용.
        // TODO. 0.5v 강화 및 Ethos 포인트, 경험치
            // 보스의 종류보다는, 순서와 Difficulty에 따라서 나누어 짐.
            // 따라서, MonoBehaviour 타입의 클래스보다 Database가 적당할 수 있음, 혹은 Den, 혹은 VillainData 
        // TODO. 0.6v 장신구
        [SerializeField] private List<GameObject> dropItemList;

        public List<GameObject> DropItemList => dropItemList;
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            dropItemList.Clear();
        }
#endif
    }
}
