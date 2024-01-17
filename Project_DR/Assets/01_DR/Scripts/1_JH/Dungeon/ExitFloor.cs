using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Unity.VisualScripting;

public class ExitFloor : MonoBehaviour
{
    IEnumerator digRoutine;
    Vector3 floorScale;
    float curHeight;             // 현재 높이
    float targetDepth;          // 목표 깊이
    public float depth = 12;    // 깊이
    public float speed = 2f;

    WaitForFixedUpdate waitForFixedUpdate;
    Vector3 floorPos;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 floor = transform.parent.transform.localScale;
        floorScale = new Vector3(floor.x, floor.y, floor.z);
        curHeight = transform.position.y;
        targetDepth = curHeight - depth;
        floorPos = transform.position;
        AudioManager.Instance.AddSFX("SFX_Drill_Digging_01");

    }


    IEnumerator Digging()
    {
        while(targetDepth < curHeight)
        {
            curHeight -= speed * Time.deltaTime ;
            floorPos.y = curHeight;
            transform.position = floorPos;
            yield return waitForFixedUpdate;

        }
        transform.gameObject.SetActive(false);
        AudioManager.Instance.PlaySFX("SFX_Drill_Digging_01");
        yield break; 

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Weapon"))
        {
            if (digRoutine != null)
            {
                return;
            }

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

            if (digRoutine != null)
            {
                StopCoroutine(digRoutine);
                digRoutine = null;
            }
        }

    }


}
