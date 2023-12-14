
namespace BossMonster
{
    public class AttackStateData_9
    {
        /*************************************************
         *                  Public Fields
         *************************************************/
        // 패턴(9) 투사체-시한폭탄 데이터
        // 추가 할 데이터 넣어도 됩니다.
        public int ProjectileType => _projectileType;           // 투사체 타입
        public int LaserCount => _laserCount;                   // 레이저 개수
        public float ProjectileDamage => _projectileDamage;     // 투사체 데미지
        public float GroundSpacing => _groundSpacing;           // 지면 간격
        public float OutputInterval => _outputInterval;         // 출력 간격
        public BossData BossData => _bossData;                  // 보스 정보


        /*************************************************
         *                Private Fields
         *************************************************/
        private int _projectileType;
        private int _laserCount;
        private float _projectileDamage;
        private float _groundSpacing;
        private float _outputInterval;
        private BossData _bossData;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자 & 부모 생성자
        public AttackStateData_9(int id, BossData bossData)
        {
            int patternID = (int)DataManager.instance.GetData(id, "AttackPatternKeyID", typeof(int));
            _bossData = bossData;
            _projectileType = (int)DataManager.instance.GetData(patternID, "ProjectileType", typeof(int));
            _laserCount = (int)DataManager.instance.GetData(patternID, "LaserCount", typeof(int));
            _projectileDamage = (float)DataManager.instance.GetData(patternID, "ProjectileDamage", typeof(float));
            _groundSpacing = (float)DataManager.instance.GetData(patternID, "GroundSpacing", typeof(float));
            _outputInterval = (float)DataManager.instance.GetData(patternID, "OutputInterval", typeof(float));
        }
    }
}
