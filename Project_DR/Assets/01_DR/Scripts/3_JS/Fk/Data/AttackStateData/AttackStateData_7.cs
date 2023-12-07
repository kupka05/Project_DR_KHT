using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMonster
{
    public class AttackStateData_7
    {
        /*************************************************
         *                  Public Fields
         *************************************************/
        // 패턴(7) 왕지팡이 데이터
        // 추가 할 데이터 넣어도 됩니다.
        public float AreaSpeed => _areaSpeed;              // 영역 속도
        public float AreaRange => _areaRange;              // 영역 범위
        public float AreaSpawnTime => _areaSpawnTime;      // 영역 생성 시간
        public float AreaDamage => _areaDamage;            // 영역 데미지
        public float OutputInterval => _outputInterval;    // 출력 간격
        public BossData BossData => _bossData;             // 보스 정보


        /*************************************************
         *                Private Fields
         *************************************************/
        private float _areaSpeed;
        private float _areaRange;
        private float _areaSpawnTime;
        private float _areaDamage;
        private float _outputInterval;
        private BossData _bossData;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자 & 부모 생성자
        public AttackStateData_7(int id, BossData bossData)
        {
            _bossData = bossData;
            _areaSpeed = (float)DataManager.instance.GetData(id, "AreaSpeed", typeof(float));
            _areaRange = (float)DataManager.instance.GetData(id, "AreaRange", typeof(float));
            _areaSpawnTime = (float)DataManager.instance.GetData(id, "AreaSpawnTime", typeof(float));
            _areaDamage = (float)DataManager.instance.GetData(id, "AreaDamage", typeof(float));
            _outputInterval = (float)DataManager.instance.GetData(id, "OutputInterval", typeof(float));
        }
    }
}
