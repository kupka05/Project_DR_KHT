using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class AttackStateData_2
    {
        /*************************************************
         *                  Public Fields
         *************************************************/
        // 패턴(2) 범위공격-나선필드 데이터
        // 추가 할 데이터 넣어도 됩니다.
        public int AreaCount => _areaCount;                                   // 영역 개수
        public float AreaRange => _areaRange;                                 // 영역 범위
        public float AreaSpeed => _areaSpeed;                                 // 영역 속도
        public float AreaSpawnTime => _areaSpawnTime;                         // 영역 생성시간
        public float ObjectDuration => _objectDuration;                       // 오브젝트 지속시간
        public float ObjectReflectionDistance => _objectReflectionDistance;   // 오브젝트 반사거리
        public float ObjectDamage => _objectDamage;                           // 오브젝트 데미지
        public float OutputInterval => _outputInterval;                       // 출력 간격
        public BossData BossData => _bossData;                                // 보스 정보


        /*************************************************s
         *                Private Fields
         *************************************************/
        private int _areaCount;
        private float _areaRange;
        private float _areaSpeed;
        private float _areaSpawnTime;
        private float _objectDuration;
        private float _objectReflectionDistance;
        private float _objectDamage;
        private float _outputInterval;
        private BossData _bossData;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자 & 부모 생성자
        public AttackStateData_2(int id, BossData bossData)
        {
            int patternID = (int)DataManager.Instance.GetData(id, "AttackPatternKeyID", typeof(int));
            _bossData = bossData;
            _areaCount = (int)DataManager.Instance.GetData(patternID, "AreaCount", typeof(int));
            _areaRange = (float)DataManager.Instance.GetData(patternID, "AreaRange", typeof(float));
            _areaSpeed = (float)DataManager.Instance.GetData(patternID, "AreaSpeed", typeof(float));
            _areaSpawnTime = (float)DataManager.Instance.GetData(patternID, "AreaSpawnTime", typeof(float));
            _objectDuration = (float)DataManager.Instance.GetData(patternID, "ObjectDuration", typeof(float));
            _objectReflectionDistance = (float)DataManager.Instance.GetData(patternID, "ObjectReflectionDistance", typeof(float));
            _objectDamage = (float)DataManager.Instance.GetData(patternID, "ObjectDamage", typeof(float));
            _outputInterval = (float)DataManager.Instance.GetData(patternID, "OutputInterval", typeof(float));
        }
    }
}
