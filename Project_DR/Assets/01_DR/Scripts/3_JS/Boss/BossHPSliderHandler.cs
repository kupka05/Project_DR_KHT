using UnityEngine.UI;

namespace BossMonster
{
    public class BossHPSliderHandler
    {
        /*************************************************
         *                Private Methods
         *************************************************/
        private Boss _boss;                 // 보스
        private BossData _bossData;         // 보스 데이터
        private Slider _bossHPSlider;       // 보스 HP 슬라이더


        /*************************************************
         *                 Public Methods
         *************************************************/
        // 생성자
        public BossHPSliderHandler(Boss boss, Slider bossHPSlider)
        {
            // 보스 및 보스 데이터 참조
            _boss = boss;
            _bossData = boss.BossData;

            // 슬라이더 연결
            _bossHPSlider = bossHPSlider;

            // Init
            Initialize();
        }

        // Init
        public void Initialize()
        {
            _bossHPSlider.maxValue = _bossData.MaxHP;
            _bossHPSlider.value = _bossData.MaxHP;
        }

        // 슬라이더 업데이트
        public void UpdateSlider()
        {
            _bossHPSlider.value = _bossData.HP;
        }
    }
}
