using BNG;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class SkillEvent : MonoBehaviour
{
    public enum Skill { Default, TeraDrill, Grinding, Landing }

    [Header("Skill")]
    public Skill skill = Skill.Default;
    public bool trigger;

    [Header("Collider")]
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;

    public float landingForce;              // 넉백 힘
    public float knockbackRange;     // 넉백 사거리

    [Header("Particle")]
    public ParticleSystem[] particles;
    public LayerMask GroundedLayers;

    [Header("Debug")]
    public float TDcheckerHeight;
    public float TDcheckerTiming;
    public float GDcheckerTiming;

    [Header("Event")]
    public UnityEvent skillEvent;
    public UnityEvent shootEnableEvent;
    public UnityEvent shootDisableEvent;
    IEnumerator skillRoutine;

    WaitForSeconds TDWaitForSeconds;
    WaitForSeconds GDWaitForSeconds;
    WaitForSeconds WaitForSeconds = new WaitForSeconds(0.25f);

    public bool bomb;
    public float bombForce;
    public float damageVal;

    // Start is called before the first frame update
    void Start()
    {
        UserData.GetData(GetData);
        DisableParticle();
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
            particles[0].gameObject.SetActive(false);
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
        yield return TDWaitForSeconds;
        skillEvent.Invoke();
    }
    IEnumerator GrinderDrill()
    {
        trigger = true;
        shootDisableEvent.Invoke();
        particles[0].gameObject.SetActive(false);
        particles[0].gameObject.SetActive(true);
        while (true)
        {
            yield return GDWaitForSeconds;
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
        sphereCollider.enabled = true;
        ActiveParticle();
        yield return WaitForSeconds;

        sphereCollider.enabled = false;

    }
    // 범위에 닿은 몬스터들의 넉백 실행부분
    public void ActiveDrillLanding(GameObject target)
    {
        target.GetComponent<Monster>().isUpper = true;

        Rigidbody targetRB = target.GetComponent<Rigidbody>();
        Vector3 skillPos = transform.localPosition;
        Vector3 targetPos = targetRB.transform.localPosition;
        skillPos.y -= 0.7f;
        //targetPos.y += 0.5f;

        Vector3 force = targetPos - skillPos * landingForce;
        if(bomb)
        {
            GFunc.Log("폭탄 터진다.");
            force = targetRB.transform.position - transform.position * bombForce;
            targetRB.AddForce(force, ForceMode.Impulse);
        }
        else
        targetRB.AddForce(force * 2, ForceMode.Impulse);

        Damageable damage = target.GetComponent<Damageable>();
        if(bomb && damage)
        {
            damage.DealDamage(damageAmount: damageVal);
        }
        else if(damage)
        {
            (float, bool) _damage = Damage.instance.DamageCalculate(UserData.GetDrillDamage(), true);
            damage.DealDamage(damageAmount : _damage.Item1, _critical: _damage.Item2);
        }
    }
    void GetData()
    {
        TDcheckerHeight = Data.GetFloat(999, "TDcheckerHeight");
        TDcheckerTiming = Data.GetFloat(999, "TDcheckerTiming");
        GDcheckerTiming = Data.GetFloat(999, "GDcheckerTiming");

        TDWaitForSeconds = new WaitForSeconds(TDcheckerTiming);
        GDWaitForSeconds = new WaitForSeconds(GDcheckerTiming);

        landingForce = UserData.GetLandingForce();
        if (skill == Skill.Landing)
        {
            AudioManager.Instance.AddSFX("SFX_Drill_Skill_Landing_Active_01");

            sphereCollider = GetComponent<SphereCollider>();

            knockbackRange = Data.GetFloat(720217, "Value1") / 2;
            sphereCollider.radius = knockbackRange;
            sphereCollider.enabled = false;
        }

        if (skill == Skill.TeraDrill)
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.center = new Vector3(0, TDcheckerHeight, 0);
        }
    }

    // 파티클을 실행시켜주는 메서드
    private void ActiveParticle()
    {
        if(particles.Length == 0)
        {
            return;
        }
        RaycastHit hit;
        Vector3 position = transform.position;
        if(Physics.Raycast(transform.position, -transform.up, out hit, 5, GroundedLayers))
        {
            position = hit.point;
            position.y += 0.5f;
        }

        for(int i = 0; i < particles.Length; i++)
        {
            GFunc.Log($"{particles[i].name} 실행 + {position}");
            particles[i].transform.position = position;
            particles[i].gameObject.SetActive(false);
            particles[i].gameObject.SetActive(true);
            particles[i].Play();
        }
        AudioManager.Instance.PlaySFX("SFX_Drill_Skill_Landing_Active_01");

    }
    private void DisableParticle()
    {
        if (particles == null)
        {
            return;
        }
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].gameObject.SetActive(false);
        }
    }
}
