using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullRoom : RandomRoom
{       // NullRoomClass는 빈방에 들어갈 Class입니다.

    void Start()
    {
        GameManager.isClearRoomList.Add(isClearRoom);
        GetFloorPos();      // 꼭지점 가져와주는 Class
    }       // Start()




    private void OnDestroy()
    {
        GameManager.isClearRoomList.Remove(isClearRoom);
        StopAllCoroutines();        // 예의치 못한 코루틴 으로 인한 이슈 방지
    }       // OnDestroy()

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(NullRoomClear());
        }
    }

    IEnumerator NullRoomClear()
    {
        yield return new WaitForSeconds(2f);
        ClearRoomBoolSetTrue();

    }

}       // ClassEnd
