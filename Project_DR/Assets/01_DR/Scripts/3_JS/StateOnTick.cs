using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>일정 주기마다 상태 관련 함수를 실행하는 클래스</summary>
public class StateOnTick : MonoBehaviour
{
    /*************************************************
     *                Public Fields
     *************************************************/
    #region [+]
    #region #Singleton
    private static StateOnTick _instance;
    public static StateOnTick Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StateOnTick>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("StateOnTick");
                    _instance = obj.AddComponent<StateOnTick>();
                }
            }
            return _instance;
        }
    }
    #endregion
    public GameObject Player => _player;

    #endregion
    /*************************************************
     *                Private Fields
     *************************************************/
    #region [+]
    private GameObject _player;                                 // 상태를 적용받을 객체
                                                                // 외부에서 객체를 가져올 수 있도록 함
    private Dictionary<int, float> _FunctionCurrentDurations;   // 함수 현재 지속시간
    private Dictionary<int, float> _FunctionMaxDurations;       // 함수 최대 지속시간

    private const float RUN_STATE_INTERVAL = 1f;                // 상태 코루틴 실행 주기(초)
    private const string CATEGORY_DURATION = "Duration";        // 구글 문서 - 지속시간 카테고리
    private const string CATEGORY_MAX_DURATION = "MaxDuration"; // 구글 문서 - 최대 지속시간 카테고리

    private Dictionary<int, Action> _functions;                 // 상태 실행 함수 저장용 리스트
    private List<int> _functionKeys;                            // 함수 키 저장

    private WaitForSeconds _waitForSeconds;                     // WaitForSeconds 캐싱(최적화)

    private bool _isProcessing = false;                         // 작업 여부를 알려주는 상태

    #endregion
    /*************************************************
     *                Unity Fields
     *************************************************/
    #region [+]
    private void Awake()
    {
        // 파괴 방지
        DontDestroyOnLoad(this);

        // 초기화
        _player = gameObject;
        _FunctionCurrentDurations = new Dictionary<int, float>();
        _FunctionMaxDurations = new Dictionary<int, float>();
        _functions = new Dictionary<int, Action>();
        _functionKeys = new List<int>();
        _waitForSeconds = new WaitForSeconds(RUN_STATE_INTERVAL);
    }

    private void Start()
    {
        // 상태 재귀 코루틴 호출
        CallStateCoroutine();
    }

    #endregion
    /*************************************************
     *                Public Methods
     *************************************************/
    #region [+]
    // StateOntick에 상태 함수를 추가한다.
    public void Add(int id, Action func)
    {
        // 상태 함수 추가
        AddFunction(id, func);
    }
  
    #endregion
    /*************************************************
     *                Private Methods
     *************************************************/
    #region [+]
    // _functions에 함수를 추가함
    // 추가된 함수는 실행 주기 마다 실행된다.
    private void AddFunction(int id, Action func)
    {
        // 아이템의 기본 지속시간 가져옴
        float effectDuration =
            (float)DataManager.Instance.GetData(id, CATEGORY_DURATION, typeof(float));

        // 함수가 이미 등록되어 있는 경우
        if (ContainsKeyInDictionary(_functions, id))
        {
            // 지속 시간 증가
            _FunctionCurrentDurations[id] += effectDuration;

            // 최대 지속 시간 초과 여부 검사 & 변경
            CheckAndUpdateDurationLimit(id);

            // 종료
            return;
        }

        // 함수 추가 & 키 저장 & 지속 시간 추가
        _functions.Add(id, func);
        _functionKeys.Add(id);
        _FunctionCurrentDurations.Add(id, effectDuration);
    }

    // _functions 내에 있는 id와 일치하는 함수와 정보를 제거
    private void RemoveFunction(int id, int index)
    {
        _functions.Remove(id);
        _functionKeys.Remove(index);
        _FunctionCurrentDurations.Remove(id);
        _FunctionMaxDurations.Remove(id);
    }

    // 함수를 실행하고 지속시간을 감소시킨다
    // 감소량은 실행 주기와 동일하다.
    private void CallFunctionAndDecreaseDuration(int id,
        float t = RUN_STATE_INTERVAL)
    {
        _functions[id]();
        _FunctionCurrentDurations[id] -= t;

        // 최대 지속 시간 초과 여부 검사 & 변경
        CheckAndUpdateDurationLimit(id);
    }

    // 함수의 지속 시간이 실행 주기 미만일 경우 삭제하는 함수
    // 미만일 경우 true 반환
    private bool DeleteFunctionIfZeroDuration(int id, int index)
    {
        // 지속 시간이 실행 주기 미만일 경우
        if (_FunctionCurrentDurations[id] < RUN_STATE_INTERVAL)
        {
            // 삭제 및 반환
            RemoveFunction(id, index);
            return true;
        }

        // 아닐 경우
        return false;
    }

    // 상태 코루틴을 호출
    private void CallStateCoroutine()
    {
        // 현재 작업중(상태 코루틴 실행)이지 않을 경우
        if (_isProcessing == false)
        {
            // 상태 변경
            _isProcessing = true;

            // 상태 코루틴 실행
            StartCoroutine(OnStateCoroutine());
        }
        else
        {
            GFunc.LogError("ItemStateManager.CallStateCoroutine()" +
                "의 재귀 호출에 문제가 발생하였습니다.");
        }
    }

    // 딕셔너리 중복 체크
    private bool ContainsKeyInDictionary<TKey, TValue>(
        Dictionary<TKey, TValue> dictionary, TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    // 최대 지속 시간 초과 여부 검사 & 변경
    public void CheckAndUpdateDurationLimit(int id)
    {
        float maxDuration = default;

        // 최대 지속 시간 정보가 없을 경우 가져옴
        if (ContainsKeyInDictionary(_FunctionMaxDurations, id) == false)
        {
            maxDuration = 
                (float)DataManager.Instance.GetData(id, CATEGORY_MAX_DURATION, typeof(float));
        }

        // 있을 경우
        else
        {
            maxDuration = _FunctionMaxDurations[id];
        }

        // 증가된 지속 시간이 최대 지속시간 이상일 경우
        if (_FunctionCurrentDurations[id] > maxDuration)
        {
            // 최대 지속 시간으로 변경
            _FunctionCurrentDurations[id] = maxDuration;
        }
    }
        
    #endregion
    /*************************************************
     *                  Coroutines
     *************************************************/
    #region [+]
    // 실행 주기 마다 계속 실행되는 재귀 코루틴
    // _functions 있는 함수를 호출한다.
    private IEnumerator OnStateCoroutine()
    {
        yield return _waitForSeconds;

        // 받아온 함수를 전부 실행
        for (int i = 0; i < _functions.Count; i++)
        {
            int id = _functionKeys[i];
            int index = i;

            // functions 딕셔너리가 유효하지 않을 경우 & 건너뛰기
            if (ContainsKeyInDictionary(_functions, id) == false) { continue; }

            // 함수의 지속시간이 실행 주기 미만일 경우 & 건너뛰기
            if (DeleteFunctionIfZeroDuration(id, index)) { continue; }

            // 아닐 경우, 함수 실행 및 지속 시간 감소
            CallFunctionAndDecreaseDuration(id);

            GFunc.Log($"{id} 지속시간: {_FunctionCurrentDurations[id]}");
        }

        // 상태 변경
        _isProcessing = false;

        // 재귀 호출
        CallStateCoroutine();
    }

    #endregion
}
