using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SkillEvent : MonoBehaviour
{
    public enum Skill { Default, TeraDrill, Grinding }

    [Header("Skill")]
    public Skill skill = Skill.Default;
    public bool trigger;

    [Header("Event")]
    public UnityEvent skillEvent;
    public UnityEvent shootEnableEvent;
    public UnityEvent shootDisableEvent;
    IEnumerator skillRoutine;

    [Header("Debug")]
    public float TDcheckerHeight;
    public float TDcheckerTiming;
    public float GDcheckerTiming;


    // Start is called before the first frame update
    void Start()
    {
        GetData();
        if (skill == Skill.TeraDrill)
        {
            transform.position = new Vector3(transform.position.x, TDcheckerHeight, transform.position.z);
        }
    }




    private void OnTriggerStay(Collider other)
    {
        if(skill == Skill.TeraDrill)
        {
            if (!other.gameObject.CompareTag("Weapon"))
            { return; }

            if (other.gameObject.GetComponent<RaycastWeaponDrill>().isSpining)
            {
                if(trigger)
                { return; }
                InitRoutine(skillRoutine);


                skillRoutine = ITeraDrill();
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

            if (other.gameObject.GetComponent<RaycastWeaponDrill>().isSpining && GetComponentInParent<RaycastWeaponDrill>().isSpining)
            {
                if (trigger)
                { return; }
                InitRoutine(skillRoutine);
                Debug.Log("시작한다..");
                skillRoutine = IGrinderDrill();
                StartCoroutine(skillRoutine);
            }
            else if(!other.gameObject.GetComponent<RaycastWeaponDrill>().isSpining || !GetComponentInParent<RaycastWeaponDrill>().isSpining)
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
            shootDisableEvent.Invoke();
            Debug.Log("트리거 해제");
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

    IEnumerator ITeraDrill()
    {
        trigger = true;   
        yield return new WaitForSeconds(TDcheckerTiming);
        skillEvent.Invoke();
    }
    IEnumerator IGrinderDrill()
    {
            trigger = true;
        shootEnableEvent.Invoke();
        while (true)
        {
            yield return new WaitForSeconds(GDcheckerTiming); ;
            skillEvent.Invoke();
        }
    }


    void GetData()
    {
        TDcheckerHeight = (float)DataManager.instance.GetData(1010, "Value1", typeof(float));
        TDcheckerTiming = (float)DataManager.instance.GetData(1010, "Value2", typeof(float));
        GDcheckerTiming = (float)DataManager.instance.GetData(20015, "Value3", typeof(float));
    }
}
