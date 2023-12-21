using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RemoteTrigger : MonoBehaviour
{

    public LayerMask RemoteCollisionLayers = 1;         // 감지 레이어

    [Header("NPC")]

    public Dictionary<Collider, NPC> ValidRemoteNPCs;   // NPC 담을 딕셔너리
    public NPC closestNPC;                              // 가장 가까운 NPC

    public float minDistance;    // 최소 감지 거리
    public float maxDistance;    // 최대 감지 거리

    [Header("Input Event")]

    public ControllerBinding triggerInput;  // 입력
    public UnityEvent triggerEvent;         // 입력 이후 추가하고 싶은 이벤트

    private Transform _eyeTransform;    // 시야 안에 들어오는지 확인
    private NPC _closest;
    private float _lastDistance;
    private float _thisDistance;

    // Start is called before the first frame update
    void Start()
    {
        ValidRemoteNPCs = new Dictionary<Collider, NPC>();

        if(Camera.main != null)
        {
            _eyeTransform = Camera.main.transform;
        }
    }

    void Update()
    {        
        if (triggerInput.GetDown())
        {
            TriggerEvent();
        }
        UpdateClosestRemoteNPC();
    }

    /// <summary>
    /// 가장 가까운 NPC를 찾는 메서드
    /// </summary>
    void UpdateClosestRemoteNPC()
    {
        closestNPC = GetClosestNPC(ValidRemoteNPCs);       
    }
    /// <summary>
    /// 가장 가까운 NPC를 찾아준다.
    /// </summary>
    /// <param name="npcs"></param>
    /// <returns>가장 가까운 NPC 리턴</returns>
    public virtual NPC GetClosestNPC(Dictionary<Collider, NPC> npcs)
    {
        _closest = null;
        _lastDistance = 9999f;

        if (npcs == null)
        {
            return null;
        }

        foreach (var npc in npcs)
        {
            // 현재 콜라이더와 NPC의 위치를 비교
            _thisDistance = Vector3.Distance(npc.Value.transform.position, transform.position);
            if (_thisDistance < _lastDistance && npc.Value.isActiveAndEnabled)
            {
                // 최소 거리가 필요할 때
                //if (_thisDistance > minDistance)
                //{
                //    continue;
                //}

                // 카메라 사이에 장애물 확인하기
                if (_eyeTransform != null)
                {
                    if (CheckObjectBetweenNPC(_eyeTransform.position, npc.Value))
                    {
                        continue;
                    }
                }

                // 가장 가까운 NPC 업데이트
                _lastDistance = _thisDistance;
                _closest = npc.Value;
            }
        }

        return _closest;
    }

    /// <summary>
    /// NPC와 카메라 사이의 장애물 체크
    /// </summary>
    /// <param name="startingPosition">현재 포지션</param>
    /// <param name="npc">검토할 NPC의 포지션</param>
    /// <returns>장애물이 있으면 false 반환</returns>
    public virtual bool CheckObjectBetweenNPC(Vector3 startingPosition, NPC npc)
    {
        RaycastHit hit;
        //GFunc.Log("위치" + npc.transform.position + "local" + npc.transform.localPosition );
        Debug.DrawLine(startingPosition, npc.transform.position, Color.red);
        if (Physics.Linecast(startingPosition, npc.transform.position, out hit, RemoteCollisionLayers, QueryTriggerInteraction.Ignore))
        {
            // Something in the way
            float hitDistance = Vector3.Distance(startingPosition, hit.point);
            if (hit.collider.gameObject != npc.gameObject)
            {
                // 거리 체크
                if (hitDistance > 0.09f)
                {
                    //GFunc.Log("Something in-between : " + hit.collider.gameObject.name + " At Distance : " + hitDistance);
                    return true;
                }
                else
                {
                    //GFunc.Log("Something in between but very close : " + hit.collider.gameObject.name);
                }
            }
        }

        return false;
    }
    // NPC 검출
    public virtual void AddValidNPC(Collider col, NPC npc)
    {
        if (col == null || npc == null)
        {
            return;
        }

        // 딕셔너리가 없으면 생성
        if (ValidRemoteNPCs == null)
        {
            ValidRemoteNPCs = new Dictionary<Collider, NPC>();
        }

        try
        {
            // NPC 추가
            if (npc != null && col != null && !ValidRemoteNPCs.ContainsKey(col))
            {
                ValidRemoteNPCs.Add(col, npc);
            }
        }
        catch (System.Exception e)
        {
            GFunc.Log("Could not add Collider " + col.transform.name + " " + e.Message);
        }
    }
    // NPC 제거
    public virtual void RemoveValidNPC(Collider col, NPC npc)
    {
        if (npc != null && ValidRemoteNPCs != null && ValidRemoteNPCs.ContainsKey(col))
        {
            ValidRemoteNPCs.Remove(col);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //  Raycast ignore 반환
        if (other.gameObject.layer == 2)
        {
            return;
        }

        //  NPC인지 확인        
        NPC npc = other.GetComponent<NPC>();
        if (npc != null)
        {
            AddValidNPC(other, npc);
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        NPC npc = other.GetComponent<NPC>();
        if (npc != null)
        {
            RemoveValidNPC(other, npc);
            return;
        }
    }

    void TriggerEvent()
    {
        triggerEvent?.Invoke(); // 이벤트 추가하고 싶은 것들 추가

        NPCEvent(); // NPC 이벤트
    }

    // NPC 관련 이벤트
    void NPCEvent()
    {
        if(closestNPC)
        {
            //GFunc.Log("NPC 이벤트 호출");
            // NPC 대화 이벤트 호출
            closestNPC.InvokeStartConverationEvent();
        }
       
    }
}
