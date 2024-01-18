using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class NullRoom : RandomRoom
{       // NullRoomClass는 빈방에 들어갈 Class입니다.

    private bool isChecking;

    public bool TestBool;
    private void Update()
    {
        TestBool = isClearRoom;
    }
    void Start()
    {

        isChecking = false;
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

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (isChecking == false)
            {
                StartCoroutine(NullRoomClear());
            }
        }
    }

    /// <summary>
    /// 모루를 생성하는 함수
    /// </summary>
    public void CreateAnvilObj()
    {
        Vector3 spawnPos = new Vector3((meshPos.bottomLeftCorner.x + meshPos.bottomRightCorner.x) * 0.5f,
            (0.5f - 0.328272f),
            (meshPos.bottomLeftCorner.z + meshPos.topLeftCorner.z) * 0.5f);
        GameObject anvilclone = Unit.CreateAnvil(spawnPos);

        anvilclone.transform.parent = this.transform;

        CreateEnhanceObj(anvilclone.transform.position);

        GFunc.Log($"모루 소환\n{this.gameObject.name}\nPos : {anvilclone.transform.position}\n SpawnObjName : {anvilclone.gameObject.name}");
    }       // CreateAnvilObj()

    /// <summary>
    /// 무기 강화소 생성
    /// </summary>
    /// <param name="_anvilPos">포지션의 기준이될 모루의 포지션</param>
    private void CreateEnhanceObj(Vector3 _anvilPos)
    {
        GameObject enhanceClone = Unit.CreateEnhance(Vector3.zero);
        Vector3 enhancePos = _anvilPos;
        enhancePos.x = enhancePos.x + 2f;
        enhancePos.y = 1.3f;          // 연구된 값
        enhanceClone.transform.position = enhancePos;
        enhanceClone.transform.parent = this.transform;

    }       // CreateEnhanceObj()

    IEnumerator NullRoomClear()
    {
        isChecking = true;
        yield return new WaitForSeconds(2f);
        ClearRoomBoolSetTrue();

    }

}       // ClassEnd
