
namespace Js.Boss
{
    public class AttackStateData_5
    {
        /*************************************************
         *                  Public Fields
         *************************************************/
        // 패턴(5) 투사체-애드벌룬 데이터
        // 추가 할 데이터 넣어도 됩니다.
        public int ProjectileType => _projectileType;              // 투사체 타입
        public int ProjectileCount => _projectileCount;            // 투사체 개수
        public float ProjectileHP => _projectileHP;                // 투사체 HP
        public float ProjectileSpeed => _projectileSpeed;          // 투사체 이동속도
        public float ProjectileDuration => _projectileDuration;    // 투사체 지속시간
        public float ProjectileDamage => _projectileDamage;        // 투사체 데미지
        public float OutputInterval => _outputInterval;            // 출력 간격
        public BossData BossData => _bossData;                     // 보스 정보


        /*************************************************
         *                Private Fields
         *************************************************/
        private int _projectileType;
        private int _projectileCount;
        private float _projectileHP;
        private float _projectileSpeed;
        private float _projectileDuration;
        private float _projectileDamage;
        private float _outputInterval;
        private BossData _bossData;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자 & 부모 생성자
        public AttackStateData_5(int id, BossData bossData)
        {
            int patternID = (int)DataManager.Instance.GetData(id, "AttackPatternKeyID", typeof(int));
            _bossData = bossData;
            _projectileType = (int)DataManager.Instance.GetData(patternID, "ProjectileType", typeof(int));
            _projectileCount = (int)DataManager.Instance.GetData(patternID, "ProjectileCount", typeof(int));
            _projectileHP = (float)DataManager.Instance.GetData(patternID, "ProjectileHP", typeof(float));
            _projectileSpeed = (float)DataManager.Instance.GetData(patternID, "ProjectileSpeed", typeof(float));
            _projectileDuration = (float)DataManager.Instance.GetData(patternID, "ProjectileDuration", typeof(float));
            _projectileDamage = (float)DataManager.Instance.GetData(patternID, "ProjectileDamage", typeof(float));
            _outputInterval = (float)DataManager.Instance.GetData(patternID, "OutputInterval", typeof(float));
        }
    }
}
