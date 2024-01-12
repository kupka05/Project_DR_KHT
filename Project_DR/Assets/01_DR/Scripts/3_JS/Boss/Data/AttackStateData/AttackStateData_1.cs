using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class AttackStateData_1
    {
        /*************************************************
         *                  Public Fields
         *************************************************/
        // 패턴(1) 화염지옥 데이터
        // 추가 할 데이터 넣어도 됩니다.
        public int AreaCount => _areaCount;                // 영역 개수
        public float AreaRange => _areaRange;              // 영역 범위
        public float AreaSpeed => _areaSpeed;              // 영역 속도
        public float AreaSpawnTime => _areaSpawnTime;      // 영역 생성시간
        public float ExplosionDamage => _explosionDamage;  // 폭발 데미지
        public float FireDamage => _fireDamage;            // 화염 데미지
        public float FireDuration => _fireDuration;        // 화염 지속시간
        public float OutputInterval => _outputInterval;    // 출력 간격
        public BossData BossData => _bossData;             // 보스 정보


        /*************************************************
         *                Private Fields
         *************************************************/
        private int _areaCount;
        private float _areaRange;
        private float _areaSpeed;
        private float _areaSpawnTime;
        private float _explosionDamage;
        private float _fireDamage;
        private float _fireDuration;
        private float _outputInterval;
        private BossData _bossData;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자 & 부모 생성자
        public AttackStateData_1(int id, BossData bossData)
        {
            int patternID = (int)DataManager.Instance.GetData(id, "AttackPatternKeyID", typeof(int));
            _bossData = bossData;
            _areaCount = (int)DataManager.Instance.GetData(patternID, "AreaCount", typeof(int));
            _areaRange = (float)DataManager.Instance.GetData(patternID, "AreaRange", typeof(float));
            _areaSpeed = (float)DataManager.Instance.GetData(patternID, "AreaSpeed", typeof(float));
            _areaSpawnTime = (float)DataManager.Instance.GetData(patternID, "AreaSpawnTime", typeof(float));
            _explosionDamage = (float)DataManager.Instance.GetData(patternID, "ExplosionDamage", typeof(float));
            _fireDamage = (float)DataManager.Instance.GetData(patternID, "FireDamage", typeof(float));
            _fireDuration = (float)DataManager.Instance.GetData(patternID, "FireDuration", typeof(float));
            _areaSpawnTime = (float)DataManager.Instance.GetData(patternID, "AreaSpawnTime", typeof(float));
            _outputInterval = (float)DataManager.Instance.GetData(patternID, "OutputInterval", typeof(float));
        }
    }
}
