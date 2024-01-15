
namespace Js.Boss
{
    public class AttackStateData_8
    {
        /*************************************************
         *                  Public Fields
         *************************************************/
        // 패턴(8) 투사체-지옥눈알 데이터
        // 추가 할 데이터 넣어도 됩니다.
        public int ProjectileType => _projectileType;           // 투사체 타입
        public int LaserCount => _laserCount;                   // 레이저 개수
        public float LaserAttackSpeed => _laserAttackSpeed;     // 레이저 공격속도
        public float LaserDamage => _laserDamage;               // 레이저 데미지
        public float OutputInterval => _outputInterval;         // 출력 간격
        public BossData BossData => _bossData;                  // 보스 정보


        /*************************************************
         *                Private Fields
         *************************************************/
        private int _projectileType;
        private int _laserCount;
        private float _laserAttackSpeed;
        private float _laserDamage;
        private float _outputInterval;
        private BossData _bossData;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자 & 부모 생성자
        public AttackStateData_8(int id, BossData bossData)
        {
            int patternID = (int)DataManager.Instance.GetData(id, "AttackPatternKeyID", typeof(int));
            _bossData = bossData;
            _projectileType = (int)DataManager.Instance.GetData(patternID, "ProjectileType", typeof(int));
            _laserCount = (int)DataManager.Instance.GetData(patternID, "LaserCount", typeof(int));
            _laserAttackSpeed = (float)DataManager.Instance.GetData(patternID, "LaserAttackSpeed", typeof(float));
            _laserDamage = (float)DataManager.Instance.GetData(patternID, "LaserDamage", typeof(float));
            _outputInterval = (float)DataManager.Instance.GetData(patternID, "OutputInterval", typeof(float));
        }
    }
}
