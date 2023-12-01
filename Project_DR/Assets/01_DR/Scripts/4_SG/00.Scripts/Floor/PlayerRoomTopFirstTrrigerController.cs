using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoomTopFirstTrrigerController : MonoBehaviour
{
    public BoxCollider boxCollider;    // 함수호출로 Get할것임



    public void GetBoxCollider(BoxCollider boxCollider_)
    {
        boxCollider = boxCollider_;
    }       // GetBoxCollider()

    public void StartTrrigerOn()
    {
        boxCollider.isTrigger = true;
        
    }       // StartTrrigerOn()

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TriggerOffCoroutine());
        }
    }

    IEnumerator TriggerOffCoroutine()
    {
        yield return new WaitForSeconds(3f);
        boxCollider.isTrigger = false;
    }

}       // ClassEnd
