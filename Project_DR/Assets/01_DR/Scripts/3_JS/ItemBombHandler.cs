using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBombHandler : MonoBehaviour
{
    /*************************************************
     *                Unity Events
     *************************************************/
    #region [+]
    private void OnCollisionEnter(Collision collision)
    {
        // 그립 후에 바닥과 충돌할 경우
        ItemColliderHandler itemHandler =
            collision.collider.GetComponent<ItemColliderHandler>();
        if (itemHandler != null)
        {
            Debug.Log(itemHandler.state);
            Debug.Log(collision.collider.tag);
            if (collision.collider.CompareTag("Floor")
                && itemHandler.state == ItemColliderHandler.State.Grabbed)
            {
                // 아이템 사용
                Debug.Log("USE");
                GetComponent<UseItem>().Use();

                itemHandler.state = ItemColliderHandler.State.Stop;
            }
        }
    }


    #endregion
    /*************************************************
     *               Public Methods
     *************************************************/
    #region [+]
    /// <summary>
    /// 폭탄을 터뜨린다.
    /// </summary>
    public void DetonateBomb(float damage)
    {
        Debug.Log("폭탄이 터진다!!!!");
        Debug.Log("진성시치도 터진다!!!");
        Debug.Log("프로젝트도 터진다!!!");

    }

    #endregion
}
