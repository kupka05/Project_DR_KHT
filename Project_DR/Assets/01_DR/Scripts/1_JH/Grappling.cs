using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private GameObject player;
    public Transform cam;
    public Transform gunTip;
    public LayerMask grappleableLayer;
    public LineRenderer lineRenderer;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;

    private Vector3 grapplePoint;

    [Header("CoolDown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    public bool grappling;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (0 < grapplingCdTimer)
            grapplingCdTimer -= Time.deltaTime;
    }


    // 그래플링 시작
    public void StartGrapple()
    {
        // 쿨다운이 안되면 리턴
        if (grapplingCdTimer > 0) return;

        grappling = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, grappleableLayer))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

    }

    private void ExecuteGrapple()
    {

    }

    private void StopGrapple()
    {
        grappling = false;
        grapplingCdTimer = grapplingCd;
    }

}
