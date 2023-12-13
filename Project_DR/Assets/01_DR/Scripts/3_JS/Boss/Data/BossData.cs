using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMonster
{
    [System.Serializable]
    public class BossData
    {
        /*************************************************
         *                  Public Fields
         *************************************************/
        // 기본 스탯
        public int ID => _id;               // 보스 아이디
        public float HP => _hp;             // 현재 체력
        public float MaxHP => _maxHP;       // 최대 체력
        public float Atk => _atk;           // 공격력
        public float Def => _def;           // 방어력
        public float GiveEXP => _giveEXP;    // 경험치
        public int GiveGold => _giveGold;    // 보상 골드


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private int _id;
        [SerializeField] private float _hp;
        [SerializeField] private float _maxHP;
        [SerializeField] private float _atk;
        [SerializeField] private float _def;
        [SerializeField] private float _giveEXP;
        [SerializeField] private int _giveGold;


        /*************************************************
         *                 Public Methods
         *************************************************/
        // 생성자
        public BossData(int id)
        {
            _id = id;
            _hp = (float)DataManager.instance.GetData(id, "HP", typeof(float));
            _maxHP = (float)DataManager.instance.GetData(id, "MaxHP", typeof(float));
            _atk = (float)DataManager.instance.GetData(id, "Atk", typeof(float));
            _def = (float)DataManager.instance.GetData(id, "Def", typeof(float));
            _giveEXP = (float)DataManager.instance.GetData(id, "GiveEXP", typeof(float));
            _giveGold = (int)DataManager.instance.GetData(id, "GiveGold", typeof(int));
        }

        // 데미지 처리
        public void OnDamage(float damage)
        {
            _hp -= damage;
        }
    }
}
