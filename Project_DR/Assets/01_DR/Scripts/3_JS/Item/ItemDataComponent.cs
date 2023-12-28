using UnityEngine;
using Rito.InventorySystem;

public class ItemDataComponent : MonoBehaviour
{
    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    public object Data => _data;
    private object _data; // Init(object data)
    private bool isUpdated = false;

    public ItemData ItemData => _itemData;
    private ItemData _itemData;
    [Header("<Debbugging> \n Double Click => Scriptable Data")]
    [SerializeField]
    private PortionItemData _potionData = default;
    [SerializeField]
    private BombItemData _bombData = default;
    [SerializeField]
    private MaterialItemData _materialData = default;
    [SerializeField]
    private QuestItemData _questData = default;

    #endregion
    /*************************************************
     *                 Public Methods
     *************************************************/
    #region [+]
    public void Initialize(object data)
    {
        Init(data);
    }

    #endregion
    /*************************************************
     *                 Public Methods
     *************************************************/
    #region [+]
    private void Init(object data)
    {
        if (isUpdated == false)
        {
            _data = data;
            isUpdated = true;

            // 인스펙터 디버깅용 함수 호출
            SetInspectorDebbuingVariable();

            // itemData 할당
            _itemData = data as ItemData;
        }
    }

    // 인스펙터에 데이터 값을 표시하기 위한 함수
    private void SetInspectorDebbuingVariable()
    {
        // C# 7.0부터 도입된 switch 패턴 매칭 사용
        // case 별로 타입을 구분한다.
        switch (_itemData)
        {
            case PortionItemData portionItemData:
                _potionData = (PortionItemData)_data;
                break;
            case BombItemData bombItemData:
                _bombData = (BombItemData)_data;
                break;
            case MaterialItemData materialItemData:
                _materialData = (MaterialItemData)_data;
                break;
            case QuestItemData questItemData:
                _questData = (QuestItemData)_data;
                break;
            default:
                break;
        }
    }

    #endregion
}
