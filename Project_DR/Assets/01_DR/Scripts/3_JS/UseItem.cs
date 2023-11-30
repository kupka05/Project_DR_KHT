using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito.InventorySystem;
using System;

public class UseItem : MonoBehaviour
{
    /*************************************************
     *                Private Fields
     *************************************************/
    #region [+]
    private ItemData _itemData;
    private bool _isProcessed = false;  // 아이템 중복 사용 방지

    //디버그
    ItemColliderHandler item;

    #endregion
    /*************************************************
     *                Unity Events
     *************************************************/
    #region [+]
    void Start()
    {
        _itemData = (ItemData)GetComponent<ItemDataComponent>().ItemData;

        // 디버그로 아이템 잡을 경우 Use 호출
        item = GetComponent<ItemColliderHandler>();
    }

    private void Update()
    {
        // 디버그로 아이템 잡을 경우 Use 호출
        if (item.state == ItemColliderHandler.State.Grabbed)
        {
            Use();
        }
    }

    #endregion
    /*************************************************
     *               Public Methods
     *************************************************/
    #region [+]
    // 현재 아이템을 사용하는 함수
    public void Use()
    {
        // 아이템이 중복 사용되지 않았을 경우
        if (_isProcessed == false)
        {
            // 아이템 상태 변경
            _isProcessed = true;

            // 현재 아이템의 타입을 가져온 후,
            // 아이템을 사용함
            Use(GetItemType());

            // 현재 오브젝트를 숨김
            InvisibleThisObject();

            // 디버그
            // 아이템 상태 매니저에 상태 추가
            Action test = () => Debug.Log("체력회복(대) 효과 발동!");
            ItemStateManager.Instance.AddFunction(5001, test);
            Action test2 = () => Debug.Log("체력회복(소) 효과 발동!");
            ItemStateManager.Instance.AddFunction(5002, test2);
            Action test3 = () => Debug.Log("방어력포션 효과 발동!");
            ItemStateManager.Instance.AddFunction(5003, test3);
            Action test4 = () => Debug.Log("폭탄(대) 효과 발동!");
            ItemStateManager.Instance.AddFunction(5101, test4);
        }
    }

    #endregion
    /*************************************************
     *               Private Methods
     *************************************************/
    #region [+]
    // 현재 아이템의 타입을 가져오는 함수
    private string GetItemType()
    {
        string type = default;

        // 포션 아이템일 경우
        if (_itemData is PortionItemData pi)
        {
            type = "Potion";
        }
        // 폭탄 아이템일 경우
        
        else if (_itemData is BombItemData bi)
        {
            type = "Bomb";
        }

        // 재료 아이템일 경우
        else if (_itemData is MaterialItemData mi)
        {
            type = "Material";
        }

        // 퀘스트 아이템일 경우
        else if (_itemData is QuestItemData qi)
        {
            type = "Quest";
        }

        return type;
    }

    // 타입에 맞는 아이템을 사용하는 함수
    private void Use(string type)
    {
        switch (type)
        {
            case "Potion":
                // 포션 아이템 사용
                UsePotionItem();
                break;

            case "Bomb":
                // 폭탄 아이템 사용
                UseBombItem();
                break;

            case "Material":
                // 재료 아이템 사용
                UseMaterialItem();
                break;

            case "Quest":
                // 퀘스트 아이템 사용
                UseQuestItem();
                break;

            default:
                // 디폴트가 나올 수 없음을 명시
                Debug.Assert(false);
                break;
        }
    }

    // 포션 아이템을 사용
    private void UsePotionItem()
    {
        // 포션 아이템 정보를 가져옴
        PortionItemData potionData = _itemData as PortionItemData;
        float effectAmount = potionData.EffectAmount;       // 회복량
        float duration = potionData.Duration;               // 지속 시간
        float maxDuration = potionData.MaxDuration;         // 최대 지속 누적시간
        float effectDuration = potionData.EffectDuration;   // 초당 회복량

        // TODO: 플레이어의 체력을 회복하는 함수 추가하기
        // Health 스크립트 참고

        // 지속 시간이 끝난 후 현재 오브젝트 삭제
        Action func = () => Destroy(gameObject);
        NewInvoke(func, duration);
    }

    // 폭탄 아이템을 사용
    private void UseBombItem()
    {
        BombItemData bombData = _itemData as BombItemData;
        float effectAmount = bombData.EffectAmount;     // 피해량 
        float duration = bombData.Duration;             // 지속 시간

        // TODO: 적의 체력을 달게하는 함수 추가하기

        // 지속 시간이 끝난 후 현재 오브젝트 삭제
        Action func = () => Destroy(gameObject);
        NewInvoke(func, duration);
    }

    // 재료 아이템을 사용
    private void UseMaterialItem()
    {
        MaterialItemData materialData = _itemData as MaterialItemData;

        // 현재 오브젝트 삭제
        Destroy(gameObject);
    }

    // 퀘스트 아이템을 사용
    private void UseQuestItem()
    {
        QuestItemData questData = _itemData as QuestItemData;

        // 현재 오브젝트 삭제
        Destroy(gameObject);
    }

    // 매개변수를 넣을 수 있는 새로운 Invoke
    private void NewInvoke(Action func, float t)
    {
        StartCoroutine(RunFuncToCoroutine(func, t));
    }

    // 현재 오브젝트의 모습을 사라지게 만듬
    private void InvisibleThisObject()
    {
        // 디버그
        gameObject.name = "*[Task:Destroy]" + " "  + gameObject.name;
        // 스케일 0으로 변경
        transform.localScale = Vector3.zero;
    }

    #endregion
    /*************************************************
     *               Private Methods
     *************************************************/
    #region [+]
    // 함수를 코루틴으로 실행하는 코루틴 함수
    private IEnumerator RunFuncToCoroutine(Action func, float t)
    {
        yield return new WaitForSeconds(t);
        func();
    }

    #endregion
}
