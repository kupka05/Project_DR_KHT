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
            smoothLocomotion.freeze = true;
        }
        RaycastHit hit;
        if(Physics.Raycast(gun.position, gun.forward, out hit, maxGrappleDistance, grappleableLayer))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = gun.position + gun.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        line.enabled = true;
        line.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        smoothLocomotion.freeze = false;
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;
        if (locomoType == LocomoType.SmoothLocomotion)
        {
            smoothLocomotion.JumpToPosition(grapplePoint, highestPointOnArc);
        }

        Invoke(nameof(StopGrapple), 1f);
    }

    private void StopGrapple()
    {
        smoothLocomotion.freeze = false;
        grappling = false;
        grapplingCdTimer = grapplingCd;
        line.enabled = false;
    }

}
