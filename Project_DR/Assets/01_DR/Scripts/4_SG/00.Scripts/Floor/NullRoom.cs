using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullRoom : RandomRoom
{       // NullRoomClass는 빈방에 들어갈 Class입니다.

    void Start()
    {
        GameManager.isClearRoomList.Add(this);
        GameManager.instance.AddNullRoom(this);
        GetFloorPos();      // 꼭지점 가져와주는 Class
    }       // Start()


    private void OnDestroy()
    {
        GameManager.instance.nullRoomList.Remove(this);
        GameManager.isClearRoomList.Remove(this);
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

    /// <summary>
    /// 모루를 생성하는 함수
    /// </summary>
    public void CreateAnvilObj()
    {
        Vector3 spawnPos = new Vector3((meshPos.bottomLeftCorner.x + meshPos.bottomRightCorner.x) * 0.5f,
            0.5f,
            (meshPos.bottomLeftCorner.z + meshPos.topLeftCorner.z) * 0.5f);
        GameObject anvilclone = Unit.CreateAnvil(spawnPos);

        anvilclone.transform.parent = this.transform;

        GFunc.Log($"모루 소환\n{this.gameObject.name}\nPos : {anvilclone.transform.position}\n SpawnObjName : {anvilclone.gameObject.name}");
    }       // CreateAnvilObj()

    IEnumerator NullRoomClear()
    {
        yield return new WaitForSeconds(2f);        
        ClearRoomBoolSetTrue();

    }

}       // ClassEnd
