using TMPro;
using UnityEngine.UI;

namespace Js.Boss
{
    public class BossHPSliderHandler
    {
        /*************************************************
         *                Private Methods
         *************************************************/
        private Boss _boss;                 // 보스
        private BossData _bossData;         // 보스 데이터
        private Slider _bossHPSlider;       // 보스 HP 슬라이더
        private TMP_Text _hpText;           // 보스 HP 텍스트


        /*************************************************
         *                 Public Methods
         *************************************************/
        // 생성자
        public BossHPSliderHandler(Boss boss)
        {
            // 보스 및 보스 데이터 참조
            _boss = boss;
            _bossData = _boss.BossData;

            // 슬라이더 연결
            _bossHPSlider = _boss.BossSummoningStone.BossHPSlider;

            // Init
            Initialize();

            // 슬라이더 업데이트
            UpdateSlider();
        }
      
        public void Initialize()
        {
            // Init
            _bossHPSlider.maxValue = _bossData.MaxHP;
            _bossHPSlider.value = _bossData.MaxHP;
            _hpText = _bossHPSlider.transform.Find("HPText").GetComponent<TMP_Text>();
        }

        // 슬라이더 업데이트
        public void UpdateSlider()
        {
            _bossHPSlider.value = _bossData.HP;
            float hp = _bossData.HP < 0 ? 0 : _bossData.HP;
            _hpText.text = GFunc.SumString(hp.ToString(), " / ", _bossData.MaxHP.ToString());
        }
    }
}
