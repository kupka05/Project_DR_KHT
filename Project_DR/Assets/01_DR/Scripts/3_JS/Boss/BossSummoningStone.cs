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
        public BossData BossData => _bossData;
        public Slider BossHPSlider => _bossHPSlider;


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private BossData _bossData;            // 보스 데이터
        [SerializeField] private Slider _bossHPSlider;          // 보스 체력바 슬라이더
        private BossHPSliderHandler bossHPSliderHandler;        // 보스 체력바 핸들러


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
        public void Initialize(BossData bossData)
        {
            // 보스 데이터 참조
            _bossData = bossData;

            // 보스 HP 캔버스 생성
            CreateBossHPCanvas();

            // 보스 HP 슬라이더 핸들러 생성
            bossHPSliderHandler = new BossHPSliderHandler(_bossData, _bossHPSlider);
        }

        // 죽음 체크
        public void IsDie()
        {
            if (_bossData.HP <= 0)
            {
                // 죽음 관련 처리
                gameObject.SetActive(false);
            }
        }

        // 데미지 처리
        public void OnDamage(float damage)
        {
            _bossData.OnDamage(damage);
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
