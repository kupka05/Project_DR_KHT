using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private GameObject player;
    private CharacterController characterController;
    private Rigidbody playerRigid;
    private SmoothLocomotion smoothLocomotion;
    private PlayerTeleport teleport;

    private PlayerGravity playerGravity;
    private PlayerClimbing playerClimbing;

    [Header("GrapplingGun")]
    public Transform gun;
    public Transform muzzleTransform;
    public LayerMask grappleableLayer;
    private LineRenderer line;


    [Header("Grappling")]
    public float maxGrappleDistance;      // 최대사거리
    public float grappleDelayTime;        // 그래플링건 딜레이
    public float GrappleReelForce = 0.5f; // 그래플링건 힘
    public float currentGrappleDistance;  // 그래플링 거리

    private Vector3 grapplePoint;

    [Header("CoolDown")]
    public float grapplingCd;
    float lastGrapplingTime;
    private bool isGrappling;
    private bool grappling;

    private void Start()
    {
         player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            characterController = player.GetComponentInChildren<CharacterController>();
            smoothLocomotion = player.GetComponentInChildren<SmoothLocomotion>();
            playerGravity = player.GetComponentInChildren<PlayerGravity>();
            playerClimbing = player.GetComponentInChildren<PlayerClimbing>();
            playerRigid = player.GetComponent<Rigidbody>();
            smoothLocomotion = player.GetComponent <SmoothLocomotion>();
        }
        else
        {
            Debug.Log("플레이어 오브젝트를 찾지 못함");
        }
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        GrapplingMove();
    }
    private void LateUpdate()
    {
        // 그래플링 중이면
        if (grappling)
        {
            // 라인 만들고
            line.SetPosition(0, muzzleTransform.position);
            // 거리 계산
            updateGrappleDistance();
        }
    }

    // 그래플링 시작
    public void StartGrapple()
    {
        if (Time.time - lastGrapplingTime < grapplingCd)
        {
            return;
        }
        grappling = true;
        smoothLocomotion.freeze = true;                 // 플레이어 이동 못하는 상태로 전환

        RaycastHit hit;
        if (Physics.Raycast(muzzleTransform.position, gun.forward, out hit, maxGrappleDistance, grappleableLayer))
        {
            grapplePoint = hit.point;                         // 충돌한 곳이 있으면 그래플링 포인트로
            lastGrapplingTime = Time.time;

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
        isGrappling = true;

        changeGravity(false);
        smoothLocomotion.freeze = false; // 정지 상태 해제
        Invoke(nameof(StopGrapple), 3f);    // 그래플링 정지

    }
    private void GrapplingMove()
    {
        if(isGrappling)
        {
            Vector3 moveDirection = (grapplePoint - muzzleTransform.position) * GrappleReelForce;
            smoothLocomotion.MoveCharacter(moveDirection * Time.deltaTime);

            if(currentGrappleDistance < 1f)
            {
                StopGrapple();
            }
        }
    }

    // 그래플링 멈춤
    public void StopGrapple()
    {
        changeGravity(true);
        smoothLocomotion.freeze = false;
        grappling = false;
        isGrappling = false;

        line.enabled = false;

        lastGrapplingTime = Time.time;
    }

    // 그래플링 포인트와 총구의 거리를 계산하는 메서드
    void updateGrappleDistance()
    {
        // Update Distance
        if (grappling)
        {
            currentGrappleDistance = Vector3.Distance(muzzleTransform.position, grapplePoint);
        }
        else
        {
            currentGrappleDistance = 0;
        }
    }
    void changeGravity(bool gravityOn)
    {
        if (playerGravity)
        {
            playerGravity.ToggleGravity(gravityOn);
        }
    }

}
