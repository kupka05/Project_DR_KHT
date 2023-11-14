using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    public enum LocomoType
    {
        Teleport,
        SmoothLocomotion,
        None
    }

    [Header("References")]
    private GameObject player;
    public Transform gun;
    public Transform gunTip;
    public LayerMask grappleableLayer;
    private LineRenderer line;

    [Header("LocomotionType")]
    public LocomotionManager locomotionManager;
    private SmoothLocomotion smoothLocomotion;
    private PlayerTeleport teleport;
    LocomoType locomoType = LocomoType.None;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("CoolDown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    private bool grappling;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        locomotionManager = player.GetComponent<LocomotionManager>();               // 플레이어 이동 매니저 가져와서
        LocomoCheck();
        line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (0 < grapplingCdTimer)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (grappling)
            line.SetPosition(0, gunTip.position);
    }
    private void LocomoCheck()
    {
        if (locomotionManager.SelectedLocomotion == LocomotionType.SmoothLocomotion)
        {
            locomoType = LocomoType.SmoothLocomotion;
            smoothLocomotion = player.GetComponent<SmoothLocomotion>();
        }
        else if (locomotionManager.SelectedLocomotion == LocomotionType.Teleport)
        {
            locomoType = LocomoType.Teleport;
            teleport = player.GetComponent<PlayerTeleport>();
        }
    }

    // 그래플링 시작
    public void StartGrapple()
    {

        // 쿨다운이 안되면 리턴
        if (grapplingCdTimer > 0) return;

        grappling = true;

        LocomoCheck(); // 쏘기전에 로코모 타입 체크
        if (locomoType == LocomoType.SmoothLocomotion)
        {
            smoothLocomotion.freeze = true;                 // 플레이어 이동 못하는 상태로 전환
        }
        RaycastHit hit;
        if(Physics.Raycast(gun.position, gun.forward, out hit, maxGrappleDistance, grappleableLayer))
        {
            grapplePoint = hit.point;                         // 충돌한 곳이 있으면 그래플링 포인트로

            Invoke(nameof(ExecuteGrapple), grappleDelayTime); // 그래플링 실행
        }
        else
        {
            grapplePoint = gun.position + gun.forward * maxGrappleDistance; // 충돌한곳이 없다면 최대사거리로 쏘고 
            Invoke(nameof(StopGrapple), grappleDelayTime);    // 그래플링 정지
        }

        line.enabled = true;                                  // 라인 켜주기
        line.SetPosition(1, grapplePoint);
    }

    // 그래플링 실행
    private void ExecuteGrapple()
    {
        smoothLocomotion.freeze = false; // 정지 상태 해제

        // lowestPoint : 플레이어의 바닥 예상 높이
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        
        // grapplePointRelativeYPos : 바닥에서 그래플링 포인트까지의 높이
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;

        // highestPointOnArc : 최대 높이
        // overshootYAxis : 최대 높이 보정점. 이 수치로 얼마나 더 튀어오를지 정할 수 있음
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        // 부드러운 이동 방식이면 smoothLocomotion에서 JumpToPosition 호출
        // 전달 변수는 그래플링 포인트와 최대 높이
        if (locomoType == LocomoType.SmoothLocomotion)
        {
            smoothLocomotion.JumpToPosition(grapplePoint, highestPointOnArc);
        }

        Invoke(nameof(StopGrapple), 1f);
    }
    // 그래플링 멈춤
    public void StopGrapple()
    {
        LocomoCheck(); // 쏘기전에 로코모 타입 체크

        smoothLocomotion.freeze = false;
        grappling = false;
        grapplingCdTimer = grapplingCd;
        line.enabled = false;
    }

}
