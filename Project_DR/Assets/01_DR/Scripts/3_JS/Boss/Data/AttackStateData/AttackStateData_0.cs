
namespace Js.Boss
{
    public class AttackStateData_0
    {
        /*************************************************
         *                  Public Fields
         *************************************************/
        // 패턴(0) 지면강타 데이터
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
        public AttackStateData_0(int id, BossData bossData)
        {
            int patternID = (int)DataManager.Instance.GetData(id, "AttackPatternKeyID", typeof(int));
            _bossData = bossData;
            _areaSpeed = (float)DataManager.Instance.GetData(patternID, "AreaSpeed", typeof(float));
            _areaRange = (float)DataManager.Instance.GetData(patternID, "AreaRange", typeof(float));
            _areaSpawnTime = (float)DataManager.Instance.GetData(patternID, "AreaSpawnTime", typeof(float));
            _areaDamage = (float)DataManager.Instance.GetData(patternID, "AreaDamage", typeof(float));
            _outputInterval = (float)DataManager.Instance.GetData(patternID, "OutputInterval", typeof(float));
        }
    }
}
