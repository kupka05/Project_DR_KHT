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
        public static event Action QuestDataCallback;               // [완료] -> DB에서 퀘스트 정보를 가져왔을 때 or 퀘스트가 완료되었을 때
        public static event Action<int, int> BossMeetCallback;      // [완료] -> [1] 보스 조우
        public static event Action<int, int> BossKillCallback;      // [완료] -> [2] 보스 킬
        public static event Action<int, int> UseItemCallback;       // [3] 아이템 사용
        public static event Action<int, int> MonsterKillCallback;   // [4] 몬스터 처치
        public static event Action<int, int> CraftingCallback;      // [5] 크래프팅
        public static event Action<int, int> ObjectCallback;        // [6] 오브젝트
        public static event Action<int, int> InventoryCallback;     // [완료] -> [7] 인벤토리(증정)
        public static event Action<int, int> DialogueCallback;      // [8] NPC와 대화


        /*************************************************
         *                Public Methods
         *************************************************/
        // DB에서 퀘스트 정보를 가져왔을 때 or 퀘스트가 완료되었을 때
        public static void OnQuestDataCallback()
        {
            QuestDataCallback?.Invoke();
        }

        // [1] 보스 조우 Callback
        public static void OnBossMeetCallback(int id, int condition = 1)
        {
            BossMeetCallback?.Invoke(id, condition);
        }

        // [2] 보스 킬 Callback
        public static void OnBossKillCallback(int id, int condition = 2)
        {
            BossKillCallback?.Invoke(id, condition);
        }

        // [3] 아이템 사용 Callback
        public static void OnUseItemCallback(int id, int condition = 3)
        {
            UseItemCallback?.Invoke(id, condition);
        }

        // [4] 몬스터 처치 Callback
        public static void OnMonsterKillCallback(int id, int condition = 4)
        {
            MonsterKillCallback?.Invoke(id, condition);
        }

        // [5] 크래프팅 Callback
        public static void OnCraftingCallback(int id, int condition = 5)
        {
            CraftingCallback?.Invoke(id, condition);
        }

        // [6] 오브젝트 Callback
        public static void OnObjectCallback(int id, int condition = 6)
        {
            ObjectCallback?.Invoke(id, condition);
        }

        // [7] 인벤토리 Callback
        public static void OnInventoryCallback(int id, int condition = 7)
        {
            InventoryCallback?.Invoke(id, condition);
        }

        // [8] NPC 대화 Callback
        public static void OnDialogueCallback(int id, int condition = 8)
        {
            DialogueCallback?.Invoke(id, condition);
        }
    }
}
