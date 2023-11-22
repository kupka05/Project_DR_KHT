using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public enum Category { Immediately, Buff, Debuff}
    public enum Skill { TeraDrill, Grinding }

    [Header("Reference")]

    public global::BNG.RaycastWeaponDrill[] drills;



    [Header("TeraDrill")]

    public bool TD_activeCheck;     // 스킬 발동 체크
    public float TD_collDown;       // 스킬 쿨다운
    public float TD_hight;          // 최소높이
    public float TD_damageIncrease; // 공격력 증가
    public float TD_drillSize;      // 드릴 크기 증가


    // Start is called before the first frame update
    void Start()
    {
        GetData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ActiveTeraDrill()
    {
        for(int i = 0; i < 2; i++)
        {
            //drills[i].
        }


    }

    IEnumerator IDeActiveTeraDrill()
    {
        yield return null;
    }



    public void GetData()
    {

    }
}
