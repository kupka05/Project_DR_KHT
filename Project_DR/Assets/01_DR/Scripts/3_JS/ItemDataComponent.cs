using UnityEngine;
using Rito.InventorySystem;

public class ItemDataComponent : MonoBehaviour
{
    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    private object _itemData; // Init(object data)
    private bool isUpdated = false;
    public object ItemData => _itemData;

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
            _itemData = data;
            isUpdated = true;

            // 인스펙터 디버깅용 함수 호출
            SetInspectorDebbuingVariable();
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
                _potionData = (PortionItemData)_itemData;
                break;
            case BombItemData bombItemData:
                _bombData = (BombItemData)_itemData;
                break;
            case MaterialItemData materialItemData:
                _materialData = (MaterialItemData)_itemData;
                break;
            case QuestItemData questItemData:
                _questData = (QuestItemData)_itemData;
                break;

            default:
                break;
        }
    }

    #endregion
}
