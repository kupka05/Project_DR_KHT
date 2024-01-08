using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class DamageUI : MonoBehaviour
{

    public TMP_Text damageUI;

    private Transform textTransform;
    private Rigidbody rigid;
    private Transform lookAt;
    private float distance;


    public float force;
    private Vector3 direction;   // 날아갈 방향

    IEnumerator disolveRoutine;
    WaitForSeconds animationDelay;

    public void Update()
    {    
        UpdateDamageUI();
    }
    public void SetUI(bool critical = default)    
    {
        textTransform = transform.GetChild(0).GetChild(0);
        damageUI = textTransform.GetComponent<TMP_Text>();
        if(critical)
        {
            damageUI.color = Color.red;
        }

        lookAt = Camera.main.transform;

        rigid = this.AddComponent<Rigidbody>();
        rigid.drag = 5;

        direction = new Vector3(-0.25f, 1f, 0);

        animationDelay = new WaitForSeconds(0.5f);
        disolveRoutine = TextDisolve();
    }

    /// <summary> 데미지를 계산하여 UI를 표시하는 메서드 </summary>
    /// <param name="damage">UI에 표시할 데미지</param>
    /// <param name="position">포지션을 변경해야 할 경우</param>
    /// <param name="left">왼쪽으로 날려야할지 여부</param>
    public void OnDeal(float damage = default, Vector3 position = default, bool left = default, bool critical = default)
    {
        SetUI(critical);
        damageUI.text = damage.ToString();
        transform.position = position;      // 위치 변경 (0이 기본값)
        DropTextAnimation(left);            // 드롭 애니메이션
        StartCoroutine(disolveRoutine);
    }

    /// <summary> 텍스트 드롭 애니메이션 </summary>
    /// <param name="left"></param>
    private void DropTextAnimation(bool left = default)
    {
        Vector3 dir;
        if (left)
        {
            dir = -lookAt.right + direction;
        }
        else
            dir = lookAt.right + direction;

         rigid.AddForce(dir * force, ForceMode.Impulse);
    }

    private void UpdateDamageUI()
    {
        if (lookAt)
        {
            textTransform.transform.LookAt(Camera.main.transform);
            textTransform.transform.forward *= -1;
            // 거리를 항상 같게 만들기
            distance = Vector3.Distance(Camera.main.transform.position, textTransform.position);
            textTransform.localScale = Vector3.one * distance;
        }
        else if (Camera.main != null)
        {
            lookAt = Camera.main.transform;
        }
        else if (Camera.main == null)
        {
            return;
        }
    }

    IEnumerator TextDisolve()
    {
        yield return animationDelay;

        Color alpha = damageUI.color;

        while(0 < damageUI.color.a)
        {
            alpha.a -= 0.1f;
            damageUI.color = alpha;
            
            if(alpha.a <= 0)
            {
                break;
            }

            yield return null;
        }
        yield return null;
        Destroy(gameObject);
    }
  
}
