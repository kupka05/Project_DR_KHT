using System.Collections.Generic;
using UnityEngine;
using Rito.InventorySystem;

public class ItemManager : MonoBehaviour
{
    /*************************************************
    *                 Public Fields
    *************************************************/
    #region [+]
    public Inventory inventory => _inventory;

    #endregion
    /*************************************************
    *                 Private Fields
    *************************************************/
    #region [+]
    [SerializeField] private Inventory _inventory;

    #endregion
    /*************************************************
    *                 Unity Events
    *************************************************/
    #region [+]
    private void Start()
    {
        ItemDataManager.InitItemDB();
        CreatePotion(5001, 99);
    }

    #endregion
    /*************************************************
    *                 Public Methods
    *************************************************/
    #region [+]
    public void ConnectInventory(Inventory inventory)
    {
        _inventory = inventory;
    }

    public void CreatePotion(int id, int amount = 1)
    {
        PortionItemData data = ItemDataManager.SearchItemDB<PortionItemData>(id);
        _inventory.Add(data, amount);
    }

    #endregion
    /*************************************************
    *                 Private Methods
    *************************************************/
    #region [+]


    #endregion
}
