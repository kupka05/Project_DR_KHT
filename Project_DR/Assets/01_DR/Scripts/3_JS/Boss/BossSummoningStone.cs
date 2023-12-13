using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BossMonster
{
    public class BossSummoningStone : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public Boss Boss => _boss;
        public BossData BossData => _bossData;
        public BNG.Damageable Damageable => _damageable;
        public Slider BossHPSlider => _bossHPSlider;


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private Boss _boss;                     // 보스
        [SerializeField] private BossData _bossData;             // 보스 데이터
        [SerializeField] private Slider _bossHPSlider;           // 보스 체력바 슬라이더
        [SerializeField] private BNG.Damageable _damageable;     // 데미지 관련 처리
        private BossHPSliderHandler _bossHPSliderHandler;        // 보스 체력바 핸들러


        /*************************************************
         *                  Unity Events
         *************************************************/
        private void Start()
        {
           
        }


        /*************************************************
         *                Public Methods
         *************************************************/
        // Init
        public void Initialize(Boss boss)
        {
            // 보스 및 보스 데이터 참조
            _boss = boss;
            _bossData = boss.BossData;

            // 데미지 관련 처리 컴포넌트 추가 및 체력 설정
            _damageable = gameObject.AddComponent<BNG.Damageable>();
            _damageable.Initialize(boss);

            // 보스 HP 캔버스 생성
            CreateBossHPCanvas();

            // 보스 HP 슬라이더 핸들러 생성
            _bossHPSliderHandler = new BossHPSliderHandler(boss, _bossHPSlider);
        }

        // 죽음 체크
        public void IsDie()
        {
            if (_bossData.HP <= 0)
            {
                // 죽음 관련 처리
                _boss.Dead();
            }
        }

        // 데미지 처리
        public void OnDamage(float damage)
        {
            Debug.Log($"데미지 {damage}");

            // 보스 데이터에 데미지 반영
            _bossData.OnDamage(damage);

            // 체력 바 업데이트
            _bossHPSliderHandler.UpdateSlider();

            // 죽음 체크
            IsDie();
        }

        /*************************************************
         *               Private Methods
         *************************************************/
        // Boss HP 캔버스 생성
        private void CreateBossHPCanvas()
        {
            // BossHP_Canvas 생성 및 _bossHPSlider 할당
            string prefabName = "BossHP_Canvas";
            GameObject canvasPrefab = Resources.Load<GameObject>(prefabName);
            GameObject canvas = Instantiate(canvasPrefab, transform);
            canvas.name = prefabName;

            _bossHPSlider = canvas.GetComponentInChildren<Slider>();
        }
    }
}
