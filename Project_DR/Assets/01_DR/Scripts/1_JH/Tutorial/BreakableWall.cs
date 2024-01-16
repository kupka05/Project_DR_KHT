using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public enum Direction { X, Y, Z }

    [Header ("Wall Setting")]
    public Direction direction; // 방향
    public bool reverse;        // 역 방향 여부

    public float depth = 12;    // 깊이
    public float speed = 2f;

    Vector3 wallPos;
    [Header("Wall Debug")]
    public float curHeight;            // 현재 높이
    public float targetDepth;          // 목표 깊이
    IEnumerator digRoutine;
    WaitForFixedUpdate waitForFixedUpdate;

    // Start is called before the first frame update
    void Start()
    {
        SetDirection();
        AudioManager.Instance.AddSFX("SFX_Drill_Digging_01");
    }

    // 방향 세팅
    private void SetDirection()
    {
        wallPos = transform.localPosition;

        // 반대일 경우 음수로 전환
        if(reverse)
        {
            depth *= -1;
        }
        // 방향에 따라 세팅
        switch (direction)
        {
            case Direction.X:
                curHeight = transform.localPosition.x;
                targetDepth = curHeight - depth;
                break;
            case Direction.Y:
                curHeight = transform.localPosition.y;
                targetDepth = curHeight - depth;
                break;
            case Direction.Z:
                curHeight = transform.localPosition.z;
                targetDepth = curHeight - depth;
                break;
        }
    }

    // 땅을 파는 부분
    IEnumerator Digging()
    {
        while (DepthCheck())
        {
            Dig();
            yield return waitForFixedUpdate;
        }
        transform.gameObject.SetActive(false);
        AudioManager.Instance.PlaySFX("SFX_Drill_Digging_01");
        yield break;
    }

    private void Dig()
    {
        float digSpeed = speed * Time.deltaTime;
        if (!reverse)
        {
            digSpeed *= -1;
        }
        curHeight += digSpeed;
        switch(direction)
        {
            case Direction.X:
            wallPos.x = curHeight;
            break;

            case Direction.Y:
            wallPos.y = curHeight;
            break;

            case Direction.Z:
            wallPos.z = curHeight;
            break;
        }
        transform.localPosition = wallPos;
    }

    private bool DepthCheck()
    {
        if(reverse)
        {
            return targetDepth > curHeight;
        }
        else
        {
            return targetDepth < curHeight;

        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            if (digRoutine != null)
            {
                return;
            }
            // 드릴이 회전중일 때만 코루틴 시작
            RaycastWeaponDrill drill = other.gameObject.GetComponent<RaycastWeaponDrill>();
            if (drill?.isSpining == true)
            {
                digRoutine = Digging();
                StartCoroutine(digRoutine);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            // 트리거 빠져 나가면 코루틴 끝내기
            if (digRoutine != null)
            {
                StopCoroutine(digRoutine);
                digRoutine = null;
            }
        }
    }
}
