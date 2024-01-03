using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Js.Quest;
using Rito.InventorySystem;
using Js.Crafting;

// 플레이어에게 특정한 명령을 실행하는 클래스
public static class Unit
{
    /*************************************************
     *         Public Inventory & Item Methods
     *************************************************/
    // 인벤토리에 아이템을 추가
    public static void AddInventoryItem(int id, int amount = 1)
    {
        ItemManager.instance.InventoryCreateItem(Vector3.zero, id, amount);
    }

    // 필드에 아이템을 생성
    public static void AddFieldItem(Vector3 pos, int id, int amount = 1)
    {
        ItemManager.instance.CreateItem(pos, id, amount);
    }

    // 모루 위에 크래프팅 아이템을 생성
    public static GameObject AddAnvilItem(Vector3 pos, int id, Transform parent, int amount = 1)
    {
        GameObject item = ItemManager.instance.CreateItem(pos, id, amount);
        Transform transform = item.transform;
        transform.SetParent(parent);
        transform.localPosition = pos;

        return item;
    }

    // 필드에 크래프팅 임시 아이템을 생성하고 반환
    public static GameObject AddFieldTempItem(Vector3 pos, int id, Transform parent, int amount = 1)
    {
        return ItemManager.instance.CreateTempItem(pos, id, parent, amount);
    }

    // ID로 인벤토리에 있는 아이템을 삭제
    public static bool RemoveInventoryItemForID(int id, int amount)
    {
        return ItemManager.instance.RemoveInventoryItemForID(id, amount);
    }

    // 인벤토리의 모든 아이템을 초기화
    public static void ResetInventory()
    {
        UserDataManager.ResetInventory();
    }


    /*************************************************
     *           Public Crafting Methods
     *************************************************/
    // 모루를 생성한다. 크래프팅용
    public static GameObject CreateAnvil(Vector3 pos)
    {
        return CraftingManager.Instance.CreateAnvil(pos);
    }

    // 강화소를 생성한다. 크래프팅용
    public static GameObject CreateEnhance(Vector3 pos)
    {
        return CraftingManager.Instance.CreateEnhance(pos);
    }


    /*************************************************
     *             Public Quest Methods
     *************************************************/
    // 데이터 테이블에 있는 퀘스트를 가져와서 생성한다
    // 기존에 가지고 있는 퀘스트가 전부 초기화된다.
    public static void CreateQuestFromDataTable()
    {
        QuestManager.Instance.CreateQuestFromDataTable();
    }

    // ID로 퀘스트를 찾아 초기 상태로 리셋한다.
    // 현재 진행 값도 초기화 된다.
    public static void ResetQuest(int id)
    {
        QuestManager.Instance.ResetQuest(id);
    }

    // ID와 일치하는 퀘스트의 상태를 [진행중]으로 변경한다.
    public static void InProgressQuestByID(int id)
    {
        Quest quest = UserDataManager.QuestDictionary[id];
        // 퀘스트가 [시작가능] 상태일 경우
        if (quest.QuestState.State.Equals(QuestState.StateQuest.CAN_STARTABLE))
        {
            // 퀘스트를 [진행중] 상태로 변경
            quest.ChangeToNextState();
        }

        // 아닐 경우
        else
        {
            GFunc.Log($"Unit.ChangeQuestStateToInProgress(): 해당 퀘스트 ID:[{id}]의 상태가 [시작가능]이 아니므로, " +
                "[진행중] 상태로 변경할 수 없습니다.");
        }
    }

    // ID와 일치하는 퀘스트를 클리어한다.
    // 성공 / 실패 이벤트 id를 반환한다.
    public static int[] ClearQuestByID(int id)
    {
        return QuestManager.Instance.GetQuestByID(id).ClearQuest();
    }

    // id를 받아서 해당 퀘스트가 [완료가능] 상태일 경우
    // true / 아닐경우 false를 반환한다.
    public static bool CheckCanCompleteQuest(int id)
    {
        // 퀘스트가 [완료가능] 상태일 경우 완료시킨다.
        // 보상도 같이 지급한다.
        Quest quest = UserDataManager.QuestDictionary[id];
        if (quest.QuestState.State.Equals(QuestState.StateQuest.CAN_COMPLETE))
        {
            return true;
        }

        // 아닐 경우
        return false;
    }



    // [시작가능] 상태의 메인 퀘스트의 첫번째 인덱스[0]를 퀘스트로 반환한다.
    public static Quest GetCanStartMainQuest()
    {
        Quest quest = GetCanStartMainQuestForList()[0];
        if (quest.Equals(null))
        {
            GFunc.LogError("받아온 퀘스트의 인덱스[0]이 null 입니다.");
        }
        return quest;
    }

    // [시작가능] 상태의 서브 퀘스트의 첫번째 인덱스[0]를 퀘스트로 반환한다.
    public static Quest GetCanStartSubQuest()
    {
        Quest quest = GetCanStartSubQuestForList()[0];
        if (quest.Equals(null))
        {
            GFunc.LogError("받아온 퀘스트의 인덱스[0]이 null 입니다.");
        }
        return quest;
    }

    // [시작가능] 상태의 특수 퀘스트의 첫번째 인덱스[0]를 퀘스트로 반환한다.
    public static Quest GetCanStartSpecialQuest()
    {
        Quest quest = GetCanStartSpeicalQuestForList()[0];
        if (quest.Equals(null))
        {
            GFunc.LogError("받아온 퀘스트의 인덱스[0]이 null 입니다.");
        }
        return quest;
    }



    // [진행중] 상태의 메인 퀘스트의 첫번째 인덱스[0]를 퀘스트로 반환한다.
    public static Quest GetInProgressMainQuest()
    {
        Quest quest = GetInProgressMainQuestForList()[0];
        if (quest.Equals(null))
        {
            GFunc.LogError("받아온 퀘스트의 인덱스[0]이 null 입니다.");
        }
        return quest;
    }

    // [진행중] 상태의 서브 퀘스트의 첫번째 인덱스[0]를 퀘스트로 반환한다.
    public static Quest GetInProgressSubQuest()
    {
        Quest quest = GetInProgressSubQuestForList()[0];
        if (quest.Equals(null))
        {
            GFunc.LogError("받아온 퀘스트의 인덱스[0]이 null 입니다.");
        }
        return quest;
    }

    // [진행중] 상태의 특수 퀘스트의 첫번째 인덱스[0]를 퀘스트로 반환한다.
    public static Quest GetInProgressSpecialQuest()
    {
        Quest quest = GetInProgressSpecialQuestForList()[0];
        if (quest.Equals(null))
        {
            GFunc.LogError("받아온 퀘스트의 인덱스[0]이 null 입니다.");
        }
        return quest;
    }


    // [완료가능] 상태의 메인 퀘스트의 첫번째 인덱스[0]를 퀘스트로 반환한다.
    public static Quest GetCanCompleteMainQuest()
    {
        Quest quest = GetCanCompleteMainQuestForList()[0];
        if (quest.Equals(null))
        {
            GFunc.LogError("받아온 퀘스트의 인덱스[0]이 null 입니다.");
        }
        GFunc.LogError($"{quest.QuestData.ID}");
        return quest;
    }

    // [완료가능] 상태의 서브 퀘스트의 첫번째 인덱스[0]를 퀘스트로 반환한다.
    public static Quest GetCanCompleteSubQuest()
    {
        Quest quest = GetCanCompleteSubQuestForList()[0];
        if (quest.Equals(null))
        {
            GFunc.LogError("받아온 퀘스트의 인덱스[0]이 null 입니다.");
        }
        return quest;
    }

    // [완료가능] 상태의 특수 퀘스트의 첫번째 인덱스[0]를 퀘스트로 반환한다.
    public static Quest GetCanCompleteSpecialQuest()
    {
        Quest quest = GetCanCompleteSpecialQuestForList()[0];
        if (quest.Equals(null))
        {
            GFunc.LogError("받아온 퀘스트의 인덱스[0]이 null 입니다.");
        }
        return quest;
    }



    // [시작가능] 상태의 메인 퀘스트를 리스트로 가져온다
    public static List<Quest> GetCanStartMainQuestForList()
    {
        // [1]메인 퀘스트 타입을 리스트로 가져옴
        List<Quest> questList = GetQuestListOfType(1);
        // 가져온 퀘스트 리스트 중에서 [1][시작가능] 상태인 퀘스트만 추출 및 반환
        return QuestManager.Instance.GetQuestsByStatusFromQuestList(questList, 1);
    }

    // [시작가능] 상태의 서브 퀘스트를 리스트로 가져온다
    public static List<Quest> GetCanStartSubQuestForList()
    {
        // [2]서브 퀘스트 타입을 리스트로 가져옴
        List<Quest> questList = GetQuestListOfType(2);
        // 가져온 퀘스트 리스트 중에서 [1][시작가능] 상태인 퀘스트만 추출 및 반환
        return QuestManager.Instance.GetQuestsByStatusFromQuestList(questList, 1);
    }

    // [시작가능] 상태의 특수 퀘스트를 리스트로 가져온다
    public static List<Quest> GetCanStartSpeicalQuestForList()
    {
        // [3]특수 퀘스트 타입를 리스트로 가져옴
        List<Quest> questList = GetQuestListOfType(3);
        // 가져온 퀘스트 리스트 중에서 [1][시작가능] 상태인 퀘스트만 추출 및 반환
        return QuestManager.Instance.GetQuestsByStatusFromQuestList(questList, 1);
    }



    // [진행중] 상태의 메인 퀘스트를 리스트로 가져온다
    public static List<Quest> GetInProgressMainQuestForList()
    {
        // [1]메인 퀘스트 타입를 리스트로 가져옴
        List<Quest> questList = GetQuestListOfType(1);
        // 가져온 퀘스트 리스트 중에서 [2][진행중] 상태인 퀘스트만 추출 및 반환
        return QuestManager.Instance.GetQuestsByStatusFromQuestList(questList, 2); 
    }

    // [진행중] 상태의 서브 퀘스트를 리스트로 가져온다
    public static List<Quest> GetInProgressSubQuestForList()
    {
        // [2]서브 퀘스트 타입를 리스트로 가져옴
        List<Quest> questList = GetQuestListOfType(2);

        // 가져온 퀘스트 리스트 중에서 [2][진행중] 상태인 퀘스트만 추출 및 반환
        return QuestManager.Instance.GetQuestsByStatusFromQuestList(questList, 2);
    }

    // [진행중] 상태의 특수 퀘스트를 리스트로 가져온다
    public static List<Quest> GetInProgressSpecialQuestForList()
    {
        // [3]특수 퀘스트 타입를 리스트로 가져옴
        List<Quest> questList = GetQuestListOfType(3);

        // 가져온 퀘스트 리스트 중에서 [2][진행중] 상태인 퀘스트만 추출 및 반환
        return QuestManager.Instance.GetQuestsByStatusFromQuestList(questList, 2);
    }



    // [완료가능] 상태의 메인 퀘스트를 리스트로 가져온다
    public static List<Quest> GetCanCompleteMainQuestForList()
    {
        // [1]메인 퀘스트 타입를 리스트로 가져옴
        List<Quest> questList = GetQuestListOfType(1);

        // 가져온 퀘스트 리스트 중에서 [3][완료가능] 상태인 퀘스트만 추출 및 반환
        return QuestManager.Instance.GetQuestsByStatusFromQuestList(questList, 3);
    }

    // [완료가능] 상태의 서브 퀘스트를 리스트로 가져온다
    public static List<Quest> GetCanCompleteSubQuestForList()
    {
        // [2]서브 퀘스트 타입를 리스트로 가져옴
        List<Quest> questList = GetQuestListOfType(2);

        // 가져온 퀘스트 리스트 중에서 [3][완료가능] 상태인 퀘스트만 추출 및 반환
        return QuestManager.Instance.GetQuestsByStatusFromQuestList(questList, 3);
    }

    // [완료가능] 상태의 특수 퀘스트를 리스트로 가져온다
    public static List<Quest> GetCanCompleteSpecialQuestForList()
    {
        // [3]특수 퀘스트 타입를 리스트로 가져옴
        List<Quest> questList = GetQuestListOfType(3);

        // 가져온 퀘스트 리스트 중에서 [3][완료가능] 상태인 퀘스트만 추출 및 반환
        return QuestManager.Instance.GetQuestsByStatusFromQuestList(questList, 3);
    }



    // 퀘스트 ID로 퀘스트를 검색하고 반환
    public static Quest GetQuestByID(int id)
    {
        return QuestManager.Instance.GetQuestByID(id);
    }

    // 인덱스로 퀘스트를 검색하고 반환
    public static Quest GetQuestByIndex(int index)
    {
        return QuestManager.Instance.GetQuestByIndex(index);
    }

    // 특정 타입의 퀘스트를 리스트로 가져온다
    // [1]=메인퀘스트, [2]=서브퀘스트, [3]=특수퀘스트
    public static List<Quest> GetQuestListOfType(int type)
    {
        return QuestManager.Instance.GetQuestsOfType(type);
    }

    // 특정한 상태에 해당하는 모든 퀘스트를 리스트로 가져온다
    public static List<Quest> GetQuestListByStatus(int state)
    {
        return QuestManager.Instance.GetQuestsByStatus(state);
    }

    // 특정 타입 퀘스트의 전체 Count를 가져온다.
    public static int GetQuestCountOfType(int type)
    {
        return QuestManager.Instance.GetQuestCountOfType(type);
    }


    /*************************************************
     *             Public QuestData Methods
     *************************************************/
    // 퀘스트가 가지고 있는 클리어 이벤트 ID중에서 특정한
    // 인덱스의 아이디를 가져온다. 매개변수 미입력시 [0] 가져옴
    public static int GetQuestClearEventID(Quest quest, int index = 0)
    {
        int id = quest.QuestData.ClearEventIDs[index];
        // id가 0일 경우 디버그 메세지 표시
        if (id.Equals(0))
        {
            GFunc.Log($"Unit.GetQuestClearEventID(): 가져온 클리어 이벤트 ID가 0입니다. 확인해주세요");
        }
        return quest.QuestData.ClearEventIDs[index];
    }

    // 퀘스트가 가지고 있는 클리어 이벤트 ID중에서 특정한
    // 인덱스의 아이디를 가져온다. 매개변수 미입력시 [0] 가져옴
    public static int GetQuestFailEventID(Quest quest, int index = 0)
    {
        int id = quest.QuestData.FailEventIDs[index];
        // id가 0일 경우 디버그 메세지 표시
        if (id.Equals(0))
        {
            GFunc.Log($"Unit.GetQuestFailEventID(): 가져온 실패 이벤트 ID가 0입니다. 확인해주세요");
        }
        return id;
    }

    // 퀘스트가 가지고 있는 클리어 이벤트 ID를 int[]로 가져온다.
    public static int[] GetQuestClearEventIDs(Quest quest)
    {
        return quest.QuestData.ClearEventIDs;
    }

    // 퀘스트가 가지고 있는 실패 이벤트 ID를 int[]로 가져온다.
    public static int[] GetQuestFailEventIDs(Quest quest)
    {
        return quest.QuestData.FailEventIDs;
    }


    /*************************************************
     *                Public DB Methods
     *************************************************/
    // 퀘스트 데이터를 DB에 저장한다
    public static void SaveQuestDataToDB()
    {
        QuestManager.Instance.SaveQuestDataToDB();
    }

    // PlayerDataManager에 있는 정보로 퀘스트 목록을 업데이트 한다.
    public static void UpdateUserQuestData()
    {
        GFunc.Log("LoadUserQuestData()");
        QuestManager.Instance.UpdateUserQuestData();
    }

    // DB에서 정보를 가져온다.
    // 가져오면서 퀘스트 생성 & 업데이트한다.
    public static void UpdateDataFromDB()
    {
        PlayerDataManager.Update();
    }
}
