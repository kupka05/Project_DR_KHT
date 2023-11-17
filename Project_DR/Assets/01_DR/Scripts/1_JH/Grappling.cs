using BNG;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class Grappling : GrabbableEvents
{
    public enum State { Idle, Shooting, Grappling };

    public State state = State.Idle;

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
    public GameObject drill;
    public GameObject drillPrefab;
    private GameObject _drill;

    [Space]

    [Header("Grappling")]
    public float maxGrappleDistance;      // 최대사거리
    public float grappleDelayTime;        // 그래플링건 딜레이
    public float GrappleReelForce = 0.5f; // 그래플링건 힘
    public float currentGrappleDistance;  // 그래플링 거리

    private Vector3 grapplePoint;
    [Space]

    [Header("CoolDown")]
    public float grapplingCd;
    float lastGrapplingTime;
    private bool isGrappling;
    public void OnEnable()
    {
        drill.SetActive(true);

    }


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
            smoothLocomotion = player.GetComponent<SmoothLocomotion>();
        }
        else
        {
            Debug.Log("플레이어 오브젝트를 찾지 못함");
        }
        GetData();

        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (state != State.Idle)
        {
            //ExcuteCheck();
            GrapplingMove();
        }
    }
    private void LateUpdate()
    {
        // 그래플링 중이면
        if (state != State.Idle)
        {
            // 라인 만들고
            line.SetPosition(0, muzzleTransform.position);
            // 거리 계산
            updateGrappleDistance();

        }
    }

    public void OnGrapple()
    {
        // 쿨타임이 안돌고 기본 상태가 아니면
        if (Time.time - lastGrapplingTime < grapplingCd)
        {
            return;
        }

        if (state == State.Idle)
        {
            StartGrapple();
        }
        else if (state == State.Shooting)
        {
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        return;
    }

    // 그래플링 시작
    public void StartGrapple()
    {
        state = State.Shooting;                         // 그래플링을 발사한 상태
        smoothLocomotion.freeze = true;                 // 플레이어 이동 못하는 상태로 전환
        input.VibrateController(0.1f, 0.2f, 0.1f, thisGrabber.HandSide);

        RaycastHit hit;
        if (Physics.Raycast(muzzleTransform.position, gun.forward, out hit, maxGrappleDistance, grappleableLayer))
        {
            grapplePoint = hit.point;                         // 충돌한 곳이 있으면 그래플링 포인트로
            lastGrapplingTime = Time.time;
            smoothLocomotion.freeze = false;
            isGrappling = true;

            if(hit.collider.GetComponent<Damageable>())
            {
                Invoke(nameof(StopGrapple), grappleDelayTime);    // 그래플링 정지
            }
        }
        else
        {
            grapplePoint = gun.position + gun.forward * maxGrappleDistance; // 충돌한곳이 없다면 최대사거리로 쏘고 
            Invoke(nameof(StopGrapple), grappleDelayTime);    // 그래플링 정지
        }

        line.enabled = true;                                  // 라인 켜주기
        line.SetPosition(1, grapplePoint);
        drill.SetActive(false);
        ShootDrill();
    }

    private void ShootDrill()
    {
        _drill = Instantiate(drillPrefab, drill.transform.position, drill.transform.rotation);
        _drill.GetComponent<DrillHead>().targetPos = grapplePoint;
    }
    public void ExcuteCheck()
    { 
        if(state == State.Shooting && !InputBridge.Instance.AButton)
        {
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);    // 그래플링 정지

        }
    }


    // 그래플링 실행
    public void ExecuteGrapple()
    {
        if (!isGrappling)
        { return; }

        state = State.Grappling;
        changeGravity(false);
    }

    // 그래플링 이동 관련
    private void GrapplingMove()
    {
        if(state == State.Grappling)
        {
            Vector3 moveDirection = (grapplePoint - muzzleTransform.position) * GrappleReelForce;
            smoothLocomotion.MoveCharacter(moveDirection * Time.deltaTime);

            if(currentGrappleDistance < 1f)
            {
                //StopGrapple();
                smoothLocomotion.freeze = true;
            }
        }
    }

    // 그래플링 멈춤
    public void StopGrapple()
    {
        //if(isGrappling)
        //{
        //    line.enabled = false;
        //    smoothLocomotion.freeze = true;
        //}

        //else
        //{ 
            changeGravity(true);
            smoothLocomotion.freeze = false;
            state = State.Idle;
            line.enabled = false;
        //}
        isGrappling = false;
        lastGrapplingTime = Time.time;
        drill.SetActive(true);
        if (_drill != null)
        {
            Destroy(_drill.gameObject);
        }

    }

    // 그래플링 포인트와 총구의 거리를 계산하는 메서드
    void updateGrappleDistance()
    {
        // Update Distance
        if (state != State.Idle)
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
    void GetData()
    {
        maxGrappleDistance = (float)DataManager.GetData(1100, "MaxDistance");
        grapplingCd = (float)DataManager.GetData(1100, "GrappleDelay");
    }
}
