using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemPurchaseHandler
{
    /*************************************************
     *               Public Methods
     *************************************************/
    #region [+]
    // 아이템을 구매하기 전에 플레이어의 골드를 체크하고
    // 아이템의 가격만큼 골드를 차감한다.
    public bool CheckAndDeductGoldForItemPurchase(int id)
    {
        int playerGold = UserDataManager.Instance.Gold;
        int price = (int)DataManager.Instance.GetData(id, "Price", typeof(int));
        // 플레이어가 아이템 구매에 적합한 골드를 소지했을 경우
        if (playerGold >= price)
        {
            // 골드 차감
            UserDataManager.Instance.Gold -= price;
            return true;
        }

        // 골드가 부족할 경우
        return false;
    }

    #endregion
}
