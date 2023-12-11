using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageStone : MonoBehaviour
{   // 해당 Class에서는 다음스테이지 이동을 위한 벽돌의 Sclae이 조절될 스크립트임

    Vector3 minusScale;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            minusScale = this.transform.localScale;
            minusScale.y -= 0.01f;
            this.transform.localScale = minusScale;
            Debug.Log($"nowY : {minusScale.y}");
            if (minusScale.y < 0.05)
            {
                Destroy(this.gameObject);
            }
        }   // if : Weapon
        //if (collision.gameObject.CompareTag("test"))
        //{

        //    minusScale = this.transform.localScale;
        //    minusScale.y -= 0.01f;
        //    this.transform.localScale = minusScale;
        //    Debug.Log($"nowY : {minusScale.y}");
        //    if (minusScale.y < 0.05)
        //    {
        //        Destroy(this.gameObject);
        //    }
        //}     // if : test
    }       // OnCollisionStay()

}
