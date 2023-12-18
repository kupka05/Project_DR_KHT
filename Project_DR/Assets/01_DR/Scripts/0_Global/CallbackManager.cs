using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackManager
{
    /*************************************************
     *                Public Fields
     *************************************************/
    public event Action<int> CallbackBossMeet;      // 보스 조우
    public event Action<int> CallbackBossKill;      // 보스 킬
    public event Action<int> CallbackUseItem;       // 아이템 사용
    public event Action<int> CallbackMonsterKill;   // 몬스터 처치
    public event Action<int> CallbackCrafting;      // 크래프팅
    public event Action<int> CallbackObject;        // 오브젝트
    public event Action<int> CallbackDialogue;      // NPC와 대화
    public event Action CallbackInventory;          // 인벤토리


    /*************************************************
     *                Public Methods
     *************************************************/
    // 보스 조우 Callback
    public void OnCallbackBossMeet(int id)
    {
        CallbackBossMeet?.Invoke(id);
        GFunc.Log("AAA");
    }

    // 보스 킬 Callback
    public void OnCallbackBossKill(int id)
    {
        CallbackBossKill?.Invoke(id);
    }

    // 아이템 사용 Callback
    public void OnCallbackUseItem(int id)
    {
        CallbackUseItem?.Invoke(id);
    }

    // 몬스터 처치 Callback
    public void OnCallbackMonsterKill(int id)
    {
        CallbackMonsterKill?.Invoke(id);
    }

    // 크래프팅 Callback
    public void OnCallbackCrafting(int id)
    {
        CallbackCrafting?.Invoke(id);
    }

    // 오브젝트 Callback
    public void OnCallbackObject(int id)
    {
        CallbackObject?.Invoke(id);
    }

    // NPC 대화 Callback
    public void OnCallbackDialogue(int id)
    {
        CallbackDialogue?.Invoke(id);
    }

    // 인벤토리 Callback
    public void OnCallbackInventory()
    {
        CallbackInventory?.Invoke();
    }

}
