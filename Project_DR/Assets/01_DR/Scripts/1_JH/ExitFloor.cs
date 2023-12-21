using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Unity.VisualScripting;

public class ExitFloor : MonoBehaviour
{
    IEnumerator digRoutine;
    Vector3 floorScale;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 floor = transform.parent.transform.localScale;
        floorScale = new Vector3(floor.x, floor.y, floor.z);
    }


    IEnumerator Digging()
    {
        while(0 <= floorScale.y)
        {
            floorScale.y -= 0.5f*Time.deltaTime ;
            transform.parent.localScale = floorScale;
            yield return new FixedUpdate();

        }
        transform.parent.gameObject.SetActive(false);

        yield break; 

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Weapon"))
        {

            GFunc.Log("드릴 감지");
            if (digRoutine != null)
            {
                return;
            }

            RaycastWeaponDrill drill = other.gameObject.GetComponent<RaycastWeaponDrill>();

            if (drill?.isSpining == true)
            {                
                return;
            }
            digRoutine = Digging();
            StartCoroutine(digRoutine);
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

    //private void OnCollisionStay(Collision other)
    //{

        //if (other.gameObject.CompareTag("Weapon"))
        //{

        //    GFunc.Log("드릴 감지");
        //    if (digRoutine != null)
        //    {
        //        return;
        //    }

        //    if(!other.gameObject.GetComponent<RaycastWeaponDrill>().isSpining)
        //    {
        //        return;
        //    }




        //    digRoutine = Digging();
        //    StartCoroutine(digRoutine);
        //}
    //}

    //private void OnCollisionExit(Collision other)
    //{
    //    if(other.gameObject.CompareTag("Weapon"))
    //    {

    //        if (digRoutine != null)
    //        {
    //            StopCoroutine(digRoutine);
    //            digRoutine = null;
    //        }
    //    }
    //}

}
