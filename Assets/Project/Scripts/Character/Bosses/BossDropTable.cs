using System.Collections.Generic;
using UnityEngine;

namespace Character.Bosses
{
    public class BossDropTable : MonoBehaviour, IEditable
    {
        // TODO. 0.4v 에서 아이템 드랍 메카닉이 정해지지 않아, 랜덤으로 count개의 아이템을 얻는 임시방식을 채용.
        [SerializeField] private List<GameObject> dropItemList;

        public List<GameObject> DropItemList => dropItemList;
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            var entireItemList = Database.EquipmentPrefabData.GetAll();
            
            dropItemList.Clear();
            dropItemList.AddRange(entireItemList);
        }
#endif
    }
}
