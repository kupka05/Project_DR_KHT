using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SkillManager>();
            }

            return m_instance;
        }
    }
    private static SkillManager m_instance; // 싱글톤이 할당될 static 변수    
    public enum Category { Immediately, Buff, Debuff}
    public enum Skill { TeraDrill, Grinding }

    [Header("Reference")]

    public GameObject[] drills;



    [Header("TeraDrill")]
    public float TD_collDown;       // 스킬 쿨다운
    public float TD_drillSize;      // 드릴 크기 증가
    IEnumerator teradrillRoutine;

    [Header("GrinderDrill")]
    public float GD_collDown;       // 스킬 쿨다운
    public float GD_addTime;        // 증가 시간
    public float GD_maxTime;        // 최대 시간
    public float GD_drillSize;      // 드릴 크기 증가
    IEnumerator grinderDrillRoutine;

    [Header("Debug")]
    public Slider grinderSlider;
    public TMP_Text grinderVal;
    IEnumerator sliderRoutine;


    // Start is called before the first frame update
    void Start()
    {
        GetData();
        SetSlider();
        StartCoroutine("Slider");
    }


    // { =======================  테라드릴  =======================
    // 테라드릴 이벤트 시작
    public void StartTeraDrill()
    {
      if(teradrillRoutine != null)
        {
            StopCoroutine(teradrillRoutine);
            teradrillRoutine = null;
        }

        teradrillRoutine = IActiveTeraDrill();
        StartCoroutine(teradrillRoutine);

    }
    // 테라드릴 실행
    IEnumerator IActiveTeraDrill()
    {
        ActiveTeraDrill();

         yield return new WaitForSeconds (TD_collDown);

        DeactiveTeraDrill();
    }
    // 테라드릴 스킬 시전
    private void ActiveTeraDrill()
    {
        VibrateManager.instance.Vibrate(0.1f, 0.2f, 0.1f,ControllerHand.Right);
        VibrateManager.instance.Vibrate(0.1f, 0.2f, 0.1f,ControllerHand.Left);
        for (int i = 0; i < 2; i++)
        {
            drills[i].GetComponent<RaycastWeaponDrill>().drillHead.transform.localScale = new Vector3(TD_drillSize, TD_drillSize, TD_drillSize);
        }
        Damage.instance.isTeradrill = true;
    }
    // 테라드릴 스킬 해제
    public void DeactiveTeraDrill()
    {
        VibrateManager.instance.Vibrate(0.1f, 0.2f, 0.1f, ControllerHand.Right);
        VibrateManager.instance.Vibrate(0.1f, 0.2f, 0.1f, ControllerHand.Left);
        for (int i = 0; i < 2; i++)
        {
            drills[i].GetComponent<RaycastWeaponDrill>().drillHead.transform.localScale = Vector3.one;
        }
        Damage.instance.isTeradrill = false;
    }
    // } =======================  테라드릴  =======================

    // { =======================  드릴 연마  =======================

    public void StartGrinderDrill()
    {
        if (grinderDrillRoutine != null)
        {
            GD_collDown += GD_addTime;          // 남은시간에 시간 추가                                             
            if (GD_collDown >= GD_maxTime)       // 시간 초과 불가하게
            {
                GD_collDown = GD_maxTime;
            }

            StopCoroutine(grinderDrillRoutine);
            grinderDrillRoutine = null;
        }

        grinderDrillRoutine = Slider();
        StartCoroutine(grinderDrillRoutine);
    }
    // 드릴연마 실행
    IEnumerator IActiveGrinderDrill()
    {
        ActiveGrinderDrill();
        yield return new WaitForSeconds(GD_collDown);
        DeActiveGrinderDrill();
    }
    // 드릴 연마 스킬 시전
    private void ActiveGrinderDrill()
    {
        
        Damage.instance.isGrinder = true;
    }
    // 드릴 연마 스킬 해제
    private void DeActiveGrinderDrill()
    {
        GD_collDown = 0;                    // 남은시간 0으로 변경
        Damage.instance.isGrinder = false;
    }
    // } =======================  드릴 연마  =======================


    private void SetSlider()
    {
        grinderSlider.maxValue = GD_maxTime;
        grinderSlider.value = 0;
    }

    IEnumerator Slider()
    {
        while (0 <= TD_collDown)
        {
            TD_collDown -= 1f * Time.deltaTime;
            if (TD_collDown <= 0)
            { TD_collDown = 0; }
            grinderSlider.value = TD_collDown;

            yield return null;
        }
        DeActiveGrinderDrill();
    }



    private void GetData()
    {
        TD_collDown = (float)DataManager.instance.GetData(20001, "Value2", typeof(float));
        TD_drillSize = (float)DataManager.instance.GetData(20002, "Value1", typeof(float));
        GD_addTime = (float)DataManager.instance.GetData(20015, "Value2", typeof(float));
        GD_maxTime = (float)DataManager.instance.GetData(20015, "Value4", typeof(float));
    }
}
