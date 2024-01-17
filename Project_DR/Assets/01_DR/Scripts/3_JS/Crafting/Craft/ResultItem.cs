using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public class ResultItem : ICraftingComponent
    {
        /*************************************************
         *                 Private Fields
         *************************************************/
        private int _itemID;
        private int _giveAmount;
        private string _itemName;
        private Vector3 _spawnPos;


        /*************************************************
         *                Public Methods
         *************************************************/
        public ResultItem(int id, Vector3 pos, int amount = 1)
        {
            // Init
            _itemID = id;
            _spawnPos = pos;
            _giveAmount = amount;
            _itemName = Data.GetString(id, "Name");
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        public bool CheckCraft()
        {
            // 조건이 없으므로 true 반환
            return true;
        }

        public bool CheckEnhance()
        {
            // 조건이 없으므로 true 반환
            return true;
        }

        public void Craft()
        {
            // 아이템 스폰
            Transform parent = CraftingManager.Instance.Anvil.transform;
            GameObject item = Unit.AddAnvilItem(_spawnPos, _itemID, parent, _giveAmount);

            // 물리효과 제거
            Rigidbody rigidbody = item.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;

            // 디버그
            GFunc.Log($"[{_itemID}]아이템 [{_itemName}]이 [{_giveAmount}] 갯수만큼 제작되었습니다.");

            // 효과음 재생
            AudioManager.Instance.AddSFX("SFX_Craft_Success_01");
            AudioManager.Instance.PlaySFX("SFX_Craft_Success_01");
        }

        public void Enhance(int type) { }
    }
}
