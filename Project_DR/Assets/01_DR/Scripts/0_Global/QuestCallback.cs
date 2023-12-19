using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    public static class QuestCallback
    {
        /*************************************************
         *                Public Fields
         *************************************************/
        public static event Action<int, int> CallbackBossMeet;      // [1] 보스 조우
        public static event Action<int, int> CallbackBossKill;      // [2] 보스 킬
        public static event Action<int, int> CallbackUseItem;       // [3] 아이템 사용
        public static event Action<int, int> CallbackMonsterKill;   // [4] 몬스터 처치
        public static event Action<int, int> CallbackCrafting;      // [5] 크래프팅
        public static event Action<int, int> CallbackObject;        // [6] 오브젝트
        public static event Action<int, int> CallbackInventory;     // [7] 인벤토리(증정)
        public static event Action<int, int> CallbackDialogue;      // [8] NPC와 대화


        /*************************************************
         *                Public Methods
         *************************************************/
        // [1] 보스 조우 Callback
        public static void OnCallbackBossMeet(int id, int condition = 1)
        {
            CallbackBossMeet?.Invoke(id, condition);
        }

        // [2] 보스 킬 Callback
        public static void OnCallbackBossKill(int id, int condition = 2)
        {
            CallbackBossKill?.Invoke(id, condition);
        }

        // [3] 아이템 사용 Callback
        public static void OnCallbackUseItem(int id, int condition = 3)
        {
            CallbackUseItem?.Invoke(id, condition);
        }

        // [4] 몬스터 처치 Callback
        public static void OnCallbackMonsterKill(int id, int condition = 4)
        {
            CallbackMonsterKill?.Invoke(id, condition);
        }

        // [5] 크래프팅 Callback
        public static void OnCallbackCrafting(int id, int condition = 5)
        {
            CallbackCrafting?.Invoke(id, condition);
        }

        // [6] 오브젝트 Callback
        public static void OnCallbackObject(int id, int condition = 6)
        {
            CallbackObject?.Invoke(id, condition);
        }


        // [8] 인벤토리 Callback
        public static void OnCallbackInventory(int id, int condition = 7)
        {
            CallbackInventory?.Invoke(id, condition);
        }

        // [7] NPC 대화 Callback
        public static void OnCallbackDialogue(int id, int condition = 8)
        {
            CallbackDialogue?.Invoke(id, condition);
        }
    }
}
