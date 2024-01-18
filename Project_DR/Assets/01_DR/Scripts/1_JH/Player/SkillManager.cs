using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    public enum Skill { TeraDrill, Grinding, Landing }

    [Header("Reference")]
    private PlayerController playerController;
    private SmoothLocomotion smoothLocomotion;
    public RaycastWeaponDrill[] drills;

    [Header("TeraDrill")]
    public float TD_collDown;       // 스킬 쿨다운
    public float TD_drillSize;      // 드릴 크기 증가
    public float TD_drillDistance;
    IEnumerator teradrillRoutine;

    [Header("GrinderDrill")]
    public float GD_coolDown;       // 스킬 쿨다운
    public float GD_addTime;        // 증가 시간
    public float GD_maxTime;        // 최대 시간
    public float GD_drillSize;      // 드릴 크기 증가
    public bool canAttack = false;  // 드릴 연마시 공격 해제
    IEnumerator grinderDrillRoutine;

    public float effectDrillSize;

    [Header("DrillLanding")]
    public SkillEvent landingSkill;
    public TMP_Text[] landingCountText;

    public int LDskillCount;            // 남은 스킬 횟수
    public bool isHookShot;             // 훅샷 사용여부
    public float activePcDistance;    // 일정 높이
    public float activeDrillDistance = 3f;    // 일정 높이

    IEnumerator checkGound;
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public float landingRange;
    public float critDamageIncre;
    public float knockBackForce;


    [Header("Debug")]
    public Slider grinderSlider;
    public TMP_Text grinderVal;
    IEnumerator sliderRoutine;

    // Start is called before the first frame update
    void Start()
    {
        UserData.GetData(GetData);

        playerController = this.transform.GetChild(0).GetChild(0).GetComponent<PlayerController>();
        smoothLocomotion = playerController.GetComponent<SmoothLocomotion>();   
    }

    private void SetGrinderSlider()
    {
        grinderSlider.maxValue = GD_maxTime;
        grinderSlider.value = 0;
        grinderVal.text = GD_coolDown.ToString();

    }

    //  #######################  테라드릴  #######################
    #region 테라드릴
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

        float drillSize = TD_drillSize + effectDrillSize;
        SetDrillSize(drillSize);
        
        Damage.instance.isTeradrill = true;
        AudioManager.Instance.PlaySFX("SFX_Drill_Skill_TeraDrill_Active_02");
    }
    // 테라드릴 스킬 해제
    public void DeactiveTeraDrill()
    {
        VibrateManager.instance.Vibrate(0.1f, 0.2f, 0.1f, ControllerHand.Right);
        VibrateManager.instance.Vibrate(0.1f, 0.2f, 0.1f, ControllerHand.Left);

        float drillSize = 1 + effectDrillSize;
        SetDrillSize(drillSize);
       
        Damage.instance.isTeradrill = false;
        AudioManager.Instance.PlaySFX("SFX_Drill_Skill_TeraDrill_End_02");
    }

    private void SetDrillSize(float size)
    {
        for (int i = 0; i < 2; i++)
        {
            drills[i].SetDrillSize(size);
        }
    }
    #endregion
    //  #######################  드릴 연마  #######################
    #region 드릴 연마
    public void StartGrinderDrill()
    {
        GD_coolDown += GD_addTime;          // 남은시간에 시간 추가                                             
        if (GD_coolDown >= GD_maxTime)       // 시간 초과 불가하게
        {
            GD_coolDown = GD_maxTime;
        }

        ActiveGrinderDrill();

        if (grinderDrillRoutine != null)
        {
            StopCoroutine(grinderDrillRoutine);
            grinderDrillRoutine = null;
        }

        grinderDrillRoutine = IActiveGrinderDrill();
        StartCoroutine(grinderDrillRoutine);
        AudioManager.Instance.PlaySFX("SFX_Drill_Skill_Grinding_Active_01");
    }
    // 드릴연마 실행
    IEnumerator IActiveGrinderDrill()
    {
        while (0 <= GD_coolDown)
        {
            GD_coolDown -= 1f * Time.deltaTime;
            
            if (GD_coolDown <= 0)
            {
                GD_coolDown = 0;
                DeActiveGrinderDrill();
            }
            
            grinderSlider.value = GD_coolDown;
            grinderVal.text = GD_coolDown.ToString();

            yield return null;
        }
    }
    // 드릴 연마 스킬 시전
    private void ActiveGrinderDrill()
    {        
        Damage.instance.isGrinder = true;
    }
    // 드릴 연마 스킬 해제
    private void DeActiveGrinderDrill()
    {
        GD_coolDown = 0;                    // 남은시간 0으로 변경
        Damage.instance.isGrinder = false;
    }

    public void ShootEnable()
    {
        if (!canAttack)
        { return; }

        for (int i = 0; i < 2; i++)
        {
            drills[i].isShootPossible = true;
        }
    }
    public void ShootDisable()
    {
        if (!canAttack)
        { return; }

        for (int i = 0; i < 2; i++)
        {
            drills[i].isShootPossible = false;
        }
    }
    #endregion
    //  #######################  드릴 랜딩  #######################
    #region 드릴 랜딩
    public void CheckLandingHeight()
    {
        if(UserData.GetDrillLandingCount() <= 0)
        {
            return;
        }

        if(activePcDistance <= playerController.DistanceFromGround)
        {
            if(checkGound != null)
            {
                StopCoroutine(checkGound);
                checkGound = null;
            }
            checkGound = CheckingGound();
            StartCoroutine(checkGound);
        }
    }
    /// <summary>
    /// 드릴 랜딩 스킬 발동 조건을 체크하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckingGound()
    {

        while (smoothLocomotion.state == PlayerState.air)
        {
            // if문 조건 필요
            // 드릴마다 각각 체크하도록 해야함
            for (int i = 0; i < 2; i ++)
            {
                // 드릴의 거리가 발동 거리 이내이고, 플레이어 또한 발동거리 이내일 경우
                if (activeDrillDistance > drills[i].distanceFromGround &&
                    playerController.DistanceFromGround < 2)
                {
                    // 만약 isKinematic이 켜져있다면, 드릴을 쥐고있는 상태가 아니다.
                    if (drills[i].GetComponent<Rigidbody>().isKinematic)
                    {
                        landingSkill.OnCollisionEvent();
                        UserData.ActiveLandingSkill();
                        landingCountText[0].text = UserData.GetDrillLandingCount().ToString();
                        landingCountText[1].text = UserData.GetDrillLandingCount().ToString();
                        yield break;
                    }
                }
         
            }
            yield return waitForEndOfFrame;
        }
        yield break;
    }
    #endregion

    //  #######################  스킬 이펙트  #######################

    public void ActiveSkill(int id, float amount = 100, float value = 0)
    {
        string effect = Data.GetString(id, "Effect");

        switch (effect) 
        {
            case "Attack":
                ActiveAttack(id, amount, value);
                return;                

            case "AttackRate":
                ActiveAttackRate(id, amount);
                return;

            case "CritDamage":
                ActiveCritDamage(id, amount);
                return;

            case "CritProbability":
                ActiveCritProbability(id, amount, value);
                return;

            case "DrillSize":
                ActiveDrillSize(id);
                return;

            case "MaxHP":
                ActiveMaxHP(id);
                return;
        }
        GFunc.Log("스킬 효과를 찾을 수 없는 ID : " + id);
    }


    // 공격력 스킬
    public void ActiveAttack(int id, float amount = 100, float value = 0)
    {        
        float attackDamage = Data.GetFloat(id, "Value1");
        if(value != 0)
        {
            attackDamage = value;
        }
        attackDamage *= (amount / 100);
        Damage.instance.AddEffectDamage(attackDamage);

        GFunc.Log($"드릴강화: 공격력 수치:[{attackDamage}]");
    }
    // 공격 속도 스킬
    public void ActiveAttackRate(int id, float amount = 100)
    {
        float attackSpeed = Data.GetFloat(id, "Value1");
        attackSpeed *= (amount / 100);

        UserDataManager.Instance.effectAttackRate += attackSpeed;

        // 드릴들의 공격속도 업데이트
        for (int i = 0; i < 2; i++)
        {
            drills[i].SetEffectFireRate(UserData.GetAttackSpeed());            
        }
    }
    // 치명타 공격력 스킬
    public void ActiveCritDamage(int id, float amount)
    {
        float critDamage = Data.GetFloat(id, "Value1");
        critDamage *= (amount / 100);
        Damage.instance.AddEffectCritDamage(critDamage);
    }
    // 치명타 확률 스킬
    public void ActiveCritProbability(int id, float amount = 100, float value = 0)
    {
        float critProbability = Data.GetFloat(id, "Value1");
        if (value != 0)
        {
            critProbability = value;
        }
        critProbability *= (amount / 100);

        Damage.instance.AddEffectCritProbability(critProbability);

        GFunc.Log($"드릴강화: 크리티컬 확률 수치:[{critProbability}]");
    }
    // 드릴 사이즈 스킬
    public void ActiveDrillSize(int id)
    {
        DeactiveTeraDrill();    // 드릴사이즈 먼저 줄이고

        float drillSize = Data.GetFloat(id, "Value1");
        effectDrillSize += drillSize;
        UserDataManager.Instance.effectDrillSize = effectDrillSize;

        SetDrillSize(effectDrillSize);
    }
    // 최대체력 스킬
    public void ActiveMaxHP(int id)
    {
        float maxHP = Data.GetFloat(id, "Value1");
        UserData.EffectMaxHP(maxHP);
        transform.GetChild(0).GetChild(0).GetComponent<PlayerHealth>().EffectMaxHPUpdate();
    }


    private void GetData()
    {
        TD_drillSize = UserData.GetTeraDrillSize();
        TD_collDown = UserData.GetTeraCoolDown();

        GD_addTime = Data.GetFloat(721114, "Value2");
        GD_maxTime = UserData.GetGrinderMaxTime();

        LDskillCount = UserData.GetDrillLandingCount();
        activePcDistance = Data.GetFloat(999, "DLActiveHeight");
        SetGrinderSlider();

        effectDrillSize = UserData.GetEffectDrillSize();

        AudioManager.Instance.AddSFX("SFX_Drill_Skill_TeraDrill_Active_02");
        AudioManager.Instance.AddSFX("SFX_Drill_Skill_TeraDrill_End_02");
        AudioManager.Instance.AddSFX("SFX_Drill_Skill_Grinding_Active_01");

        landingCountText[0].text = LDskillCount.ToString();
        landingCountText[1].text = LDskillCount.ToString();

        //TD_collDown = Data.GetFloat(721100, "Value2");
        //TD_drillSize = Data.GetFloat(721100, "Value1");

        //GD_addTime = Data.GetFloat(721114, "Value2");
        //GD_maxTime = Data.GetFloat(721114, "Value4");
    }
}
