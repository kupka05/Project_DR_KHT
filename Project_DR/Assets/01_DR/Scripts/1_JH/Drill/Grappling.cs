using BNG;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class Grappling : GrabbableEvents
{
    public enum State { Idle, Shooting, Grappling };

    public State state = State.Idle;

    [Header("References")]
    public GameObject player;
    private CharacterController characterController;
    public Rigidbody playerRigid;
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

    private bool isDamageCheck;
    private Damageable target;

    public TMP_Text debug;
    public TMP_Text debug2;

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
#if JH_DEBUG
        DebugMode();
#endif
   
    }
    private void FixedUpdate()
    {
        if (state != State.Grappling)
            return;

        GrapplingMove(); // 그래플링일 경우에만 실행

    }
#if JH_DEBUG

    public void DebugMode()
    {
        debug.gameObject.SetActive(true);
        debug2.gameObject.SetActive(true);
        debug.text = string.Format(""+state);
        debug2.text = string.Format(currentGrappleDistance+ "m");

    }
#endif

    private void LateUpdate()
    {
        

        // 그래플링 중이면
        if (state != State.Idle)
        {
            // 라인 만들고
            line.SetPosition(0, muzzleTransform.position);
            // 거리 계산
        }
        updateGrappleDistance();
        DamageCheck(); // 데미지 체크
        GrapleDisable();
    }

    // 그래플링 쏘기와 당기기를 한 버튼으로 할 경우
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
        if (state != State.Idle || Time.time - lastGrapplingTime < grapplingCd)
            return;
        lastGrapplingTime = Time.time;


        state = State.Shooting;                         // 그래플링을 발사한 상태
        //smoothLocomotion.freeze = true;                 // 플레이어 이동 못하는 상태로 전환
        input.VibrateController(0.1f, 0.2f, 0.1f, thisGrabber.HandSide);
        RaycastHit hit;
        // 레이 발사
        if (Physics.Raycast(muzzleTransform.position, gun.forward, out hit, maxGrappleDistance, grappleableLayer))
        {
            grapplePoint = hit.point;                         // 충돌한 곳이 있으면 그래플링 포인트로
            smoothLocomotion.freeze = false;                  // 플레이어 움직일 수 있도록 전환

            // 만약  damageable 오브젝트와 충돌 시
            if (hit.collider.GetComponent<Damageable>())
            {
                isDamageCheck = true;                             // 데미지 체크 켜고
                target = hit.collider.GetComponent<Damageable>(); // 타겟 세팅
            }
        }

        else
        {
            grapplePoint = gun.position + gun.forward * maxGrappleDistance; // 충돌한곳이 없다면 최대사거리로 쏘고 
            Invoke(nameof(StopGrapple), grappleDelayTime);                  // 그래플링 정지
        }

        line.enabled = true;                                  // 라인 켜주기
        line.SetPosition(1, grapplePoint);                    // 그래플링 포인트까지
        drill.SetActive(false);                               // 달려있는 드릴 잠깐 꺼주고
        ShootDrill();                                         // 그래플링용 드릴 발사
    }


    // 드릴 발사
    private void ShootDrill()
    {
        _drill = Instantiate(drillPrefab, drill.transform.position, drill.transform.rotation); // 드릴 인스턴스
        _drill.transform.localScale = drill.transform.localScale;
        _drill.transform.LookAt(grapplePoint);                                                 // 타겟 바라보고
        _drill.GetComponent<DrillHead>().targetPos = grapplePoint;                             // 그래플링 포인트 세팅
        _drill.GetComponent<DrillHead>().grappling = this;                                     // 드릴이 만난 오브젝트의 체력이 0이하일 경우를 체크하기 위함
        Destroy(_drill,grappleDelayTime);
    }
    

    // 그래플링 실행
    public void ExecuteGrapple()
    {
        if (Time.time - lastGrapplingTime < grapplingCd)
            Invoke(nameof(Excute), grappleDelayTime);                  // 그래플링 실행

        else
            Excute();
    }
    public void Excute()
    {
        if (state != State.Shooting)
        { return; }
        state = State.Grappling;
        changeGravity(false);
        //smoothLocomotion.freeze = true;
        smoothLocomotion.grappleCount++;
    }

    // 그래플링 이동 관련 : state가 그래플링일 경우 실행
    private void GrapplingMove()
    {


        if (currentGrappleDistance < 0.3f)
            return;
        // 플레이어 이동
        Vector3 moveDirection = (grapplePoint - muzzleTransform.position) * GrappleReelForce;
        //smoothLocomotion.MoveCharacter(moveDirection * Time.deltaTime);

        playerRigid.velocity = moveDirection;
        //playerRigid.MovePosition(grapplePoint);
    }

    // 그래플링 멈춤
    public void StopGrapple()
    {
        if (state == State.Idle)
            return;
        if (smoothLocomotion.grappleCount == 1)
        {
            changeGravity(true);
            smoothLocomotion.freeze = false;
        }
        if (state == State.Grappling)
        {
            smoothLocomotion.grappleCount--;
        }

        
        state = State.Idle;

        lastGrapplingTime = Time.time;

        drill.SetActive(true);
        line.enabled = false;
        
        isDamageCheck = false;
        target = null;



        //if (_drill != null)
        //{
        //    Destroy(_drill.gameObject);
        //}

    }
    public void GrapleDisable()
    {
        if (currentGrappleDistance < 1.8f && state == State.Grappling)
        {
            StopGrapple();
        }
    }

    // 데미지를 체크하는 메서드
    public void DamageCheck()
    {
        if (!isDamageCheck)
            return;
        if (target.Health <= 0)
        {
            StopGrapple(); // 맞은 상대가 체력이 0이하이면, 그래플링 멈추기
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

    // 중력 변경
    void changeGravity(bool gravityOn)
    {
        if (playerGravity)
        {
            playerGravity.ToggleGravity(gravityOn);
        }
        playerRigid.useGravity = gravityOn;
    }
    // 데이터 가져오기
    void GetData()
    {
        maxGrappleDistance = (float)DataManager.instance.GetData(1100, "MaxDistance", typeof(float));
        grapplingCd = (float)DataManager.instance.GetData(1100, "GrappleDelay", typeof(float)) ;
    }

    public void SetRigid()
    {
        if (playerRigid != null)
        { return; }

        playerRigid = player.GetComponent<Rigidbody>();
    }
}
