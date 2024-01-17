using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Js.Boss
{
    public class BossSummoningStone : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public enum Phase
        {
            ONE = 0,        // 1 페이즈
            TWO = 1,        // 2 페이즈
            THREE = 2,      // 3 페이즈
            FOUR = 3        // 4 페이즈
        }
        public Boss Boss => _boss;
        public Old_Boss OldBoss => _oldBoss;
        public BossNPC BossNPC => _bossNPC;
        public BossData BossData => _bossData;
        public BNG.Damageable Damageable => _damageable;
        public Slider BossHPSlider => _bossHPSlider;
        public Phase CurrentPhase => _currentPhase;
        public BossPhaseHandler BossPhaseHandler => _bossPhaseHandler;
        

        /*************************************************
         *                 Private Fields
         *************************************************/
        [Header("BossSummoningStone")]
        [SerializeField] private Boss _boss;                                    // 보스
        [SerializeField] private Old_Boss _oldBoss;                             // 구버전 보스
        [SerializeField] private BossNPC _bossNPC;                              // 보스 NPC
        [SerializeField] private BossData _bossData;                            // 보스 데이터
        [SerializeField] private Slider _bossHPSlider;                          // 보스 체력바 슬라이더
        [SerializeField] private BNG.Damageable _damageable;                    // 데미지 관련 처리
        [SerializeField] private BossHPSliderHandler _bossHPSliderHandler;      // 보스 체력바 핸들러
        [SerializeField] private Image _imgIcon_1;                              // 아이콘 (1)
        [SerializeField] private Image _imgIcon_2;                              // 아이콘 (2)
        [SerializeField] private GameObject _gameStartObject;                   // 게임 시작 오브젝트
        private BossPhaseHandler _bossPhaseHandler;                             // 보스 페이즈 핸들러
        private Phase _currentPhase;                                            // 현재 페이즈


        /*************************************************
         *                Public Methods
         *************************************************/
        // Init
        public void Initialize(Boss boss)
        {
            // 보스 및 보스 데이터 참조
            _boss = boss;
            _oldBoss = GetComponent<Old_Boss>();
            _bossData = boss.BossData;

            // 데미지 관련 처리 컴포넌트 호출 및 체력 설정
            // 원래는 호출이 아니라 Add로 추가하는데 Damageable의 경우
            // OnDamaged 이벤트에 하나라도 추가가 안되있을 경우 오류가 발생 
            _damageable = gameObject.GetComponent<BNG.Damageable>();
            _damageable.Initialize();

            // 보스 HP 슬라이더 핸들러 생성
            _bossHPSliderHandler = new BossHPSliderHandler(boss);

            // 보스 페이즈 핸들러 생성
            _bossPhaseHandler = new BossPhaseHandler(boss);

            // HP바가 항상 플레이어를 바라보도록 컴포넌트 추가
            LookAtTarget lookAtTarget = _bossHPSlider.transform.parent.parent.
                gameObject.AddComponent<LookAtTarget>();

            // Old_Boss의 기본 설정과 공격 주체를 Init
            _oldBoss.InitializeBoss();
            _oldBoss.InitializeTransform(_boss.BossMonster.transform);

            // 보스NPC Init & NPC 트리거 설정
            _bossNPC = GetComponent<BossNPC>();
            _boss.SetNPCTrigger();
        }

        // 현재 페이즈 변경
        public void ChangeCurrentPhase(Phase phase)
        {
            // 현재 페이즈와 변경 할 페이즈가 다른 경우
            if (_currentPhase != phase)
            {
                GFunc.Log($"페이즈를 {phase}로 변경");

                // 페이즈 변경
                _currentPhase = phase;
                // 보스 데이터의 현재 패턴 갯수 변경
                _boss.BossData.SetCurrentPatternCount((int)phase);
                // 공격 패턴 변경
                _bossData.ChooseRandomPattern();

                // 플레이어를 보스룸 입구로 텔레포트
                GameManager.instance.EndBossCutScene();
            }
        }

        // 데미지 처리
        public void OnDamage(float damage)
        {
            GFunc.Log($"데미지 {damage}");

            // 보스 데이터에 데미지 반영
            _bossData.OnDamage(damage);

            // 체력 바 업데이트
            _bossHPSliderHandler.UpdateSlider();

            // 죽음 체크
            IsDie();

            // 페이즈 체크
            IsInPhase();
        }

        // 부모와 포지션을 변경
        public void SetParentAndPosition(Transform parent)
        {
            transform.SetParent(parent.parent);
            Vector3 position = parent.transform.position;
            position.z -= 3.0f;
            position.y -= 2.0f;
            transform.position = position;
        }
        

        /*************************************************
         *               Private Methods
         *************************************************/
        //LEGACY:
        //// Boss HP 캔버스 생성
        //private void CreateBossHPCanvas()
        //{
        //    // BossHP_Canvas 생성 및 _bossHPSlider 할당
        //    string prefabName = "BossHP_Canvas";
        //    GameObject canvasPrefab = Resources.Load<GameObject>(prefabName);
        //    GameObject canvas = Instantiate(canvasPrefab, transform);
        //    canvas.name = prefabName;

        //    // HP바 위치/각도 조정
        //    _bossHPSlider = canvas.GetComponentInChildren<Slider>();
        //    Vector3 position = _bossHPSlider.transform.position;
        //    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        //    position.x = 0f;
        //    _bossHPSlider.transform.position = position;
        //    _bossHPSlider.transform.rotation = rotation;
        //}

        // 죽음 체크
        private void IsDie()
        {
            if (_bossData.HP <= 0)
            {
                // 죽음 관련 처리
                _boss.Dead();

                // LEGACY:
                // 0.1초 후 소환석 오브젝트 삭제
                //Destroy(gameObject, 0.1f);
                // 소환석 숨기기
                gameObject.transform.localScale = Vector3.zero;
            }
        }

        // 페이즈 체크
        private void IsInPhase()
        {
            _bossPhaseHandler.IsInPhase();
        }
    }
}
