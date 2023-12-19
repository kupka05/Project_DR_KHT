using BNG;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SkillEvent : MonoBehaviour
{
    public enum Skill { Default, TeraDrill, Grinding, Landing }

    [Header("Skill")]
    public Skill skill = Skill.Default;
    public bool trigger;

    [Header("Landing")]
    private SphereCollider sphereCollider;

    public float landingForce;              // 넉백 힘
    public float knockbackRange;     // 넉백 사거리

    [Header("Debug")]
    public float TDcheckerHeight;
    public float TDcheckerTiming;
    public float GDcheckerTiming;

    [Header("Event")]
    public UnityEvent skillEvent;
    public UnityEvent shootEnableEvent;
    public UnityEvent shootDisableEvent;
    IEnumerator skillRoutine;

    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();       
        UserData.GetData(GetData);      
    }




    private void OnTriggerStay(Collider other)
    {
        if(skill == Skill.TeraDrill)
        {
            if (!other.gameObject.CompareTag("Weapon"))
            { return; }

            if (other.gameObject.GetComponentInParent<RaycastWeaponDrill>().isSpining)
            {
                if(trigger)
                { return; }
                InitRoutine(skillRoutine);


                skillRoutine = TeraDrill();
                StartCoroutine(skillRoutine);
            }
            else
            {
                InitRoutine(skillRoutine);
            }
            
        }

        if(skill == Skill.Grinding)
        {

            if (!other.gameObject.CompareTag("Weapon"))
            { return; }

            if (other.gameObject.GetComponentInParent<RaycastWeaponDrill>().isSpining && GetComponentInParent<RaycastWeaponDrill>().isSpining)
            {
                if (trigger)
                { return; }
                InitRoutine(skillRoutine);
                skillRoutine = GrinderDrill();
                StartCoroutine(skillRoutine);
            }
            else if(!other.gameObject.GetComponentInParent<RaycastWeaponDrill>().isSpining || !GetComponentInParent<RaycastWeaponDrill>().isSpining)
            {
                if (skillRoutine != null)
                {
                    StopCoroutine(skillRoutine);
                }
            }
        }       
    }
    private void OnTriggerExit(Collider other)
    {
        if (skill == Skill.TeraDrill)
        {
            if (!other.gameObject.CompareTag("Weapon"))
            { return; }
            InitRoutine(skillRoutine);
        }
        if (skill == Skill.Grinding)
        {
            if (!other.gameObject.CompareTag("Weapon"))
            { return; }
            InitRoutine(skillRoutine);
            shootEnableEvent.Invoke();
        }
    }
    private void InitRoutine(IEnumerator routine)
    {
        trigger = false;

        if (routine != null)
        {
            StopCoroutine(routine);
        }
    }

    IEnumerator TeraDrill()
    {
        trigger = true;   
        yield return new WaitForSeconds(TDcheckerTiming);
        skillEvent.Invoke();
    }
    IEnumerator GrinderDrill()
    {
            trigger = true;
        shootDisableEvent.Invoke();
        while (true)
        {
            yield return new WaitForSeconds(GDcheckerTiming); ;
            skillEvent.Invoke();
        }
    }

    // 스킬 발동 조건을 만족하면 일어나는 이벤트
    public void OnCollisionEvent()
    {
        if(skillRoutine != null)
        {
            StopCoroutine (skillRoutine);
            skillRoutine = null;
        }

        skillRoutine = DrillLanding();
        StartCoroutine(skillRoutine);
    }
    // 넉백 범위의 콜라이더 껐다 켜주기
    IEnumerator DrillLanding()
    {
        GFunc.Log("On");
        sphereCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);

        GFunc.Log("Off");
        sphereCollider.enabled = false;

    }
    // 범위에 닿은 몬스터들의 넉백 실행부분
    public void ActiveDrillLanding(GameObject target)
    {
        Rigidbody targetRB = target.GetComponent<Rigidbody>();
        Vector3 skillPos = transform.localPosition;
        Vector3 targetPos = targetRB.transform.localPosition;
        skillPos.y -= 0.7f;
        //targetPos.y += 0.5f;

        Vector3 force = targetPos - skillPos * landingForce;
        targetRB.AddForce(force, ForceMode.Impulse);

        Damageable damage = target.GetComponent<Damageable>();
        if(damage)
        {
            damage.DealDamage(Damage.instance.DamageCalculate(UserData.GetDrillDamage()));
        }
    }
    void GetData()
    {
        TDcheckerHeight = Data.GetFloat(1010, "Value1");
        TDcheckerTiming = Data.GetFloat(1010, "Value2");
        GDcheckerTiming = Data.GetFloat(20015, "Value3");
        //TDcheckerHeight = (float)DataManager.instance.GetData(1010, "Value1", typeof(float));
        //TDcheckerTiming = (float)DataManager.instance.GetData(1010, "Value2", typeof(float));
        //GDcheckerTiming = (float)DataManager.instance.GetData(20015, "Value3", typeof(float));

        landingForce = UserData.GetLandingForce();

        if (skill == Skill.Landing)
        {
            knockbackRange = Data.GetFloat(720217, "Value1") / 2;
            sphereCollider.radius = knockbackRange;
            sphereCollider.enabled = false;
        }
    }
}
