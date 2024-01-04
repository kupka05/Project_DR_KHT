using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDoor : MonoBehaviour
{
    /*************************************************
     *                Public Fields
     *************************************************/
    public enum State
    {
        OPEN = 0,       // 열린 상태
        CLOSE = 1       // 닫힌 상태
    }
    [SerializeField] public State CurrentState => _currentState;            // 현재 상태
    [SerializeField] public Animation Animation => _animation;              // 애니메이션
    [SerializeField] public Coroutine Coroutine => _coroutine;              // 코루틴


    /*************************************************
     *                Private Fields
     *************************************************/
    [SerializeField] private State _currentState;
    [SerializeField] private Animation _animation;
    [SerializeField] Coroutine _coroutine;                                  // 코루틴
    [SerializeField] WaitForSeconds _delay = new WaitForSeconds(1.0f);      // 코루틴 딜레이


    /*************************************************
     *                 Unity Events
     *************************************************/
    void Start()
    {
        // Init
        Initialize();
    }


    /*************************************************
     *                 Public Methods
     *************************************************/
    public void Initialize()
    {
        // Init
        _animation = transform.Find("Door_Middle").GetComponent<Animation>();
    }

    // 현재 상태 변경
    public void SetCurrentState(State state)
    {
        _currentState = state;
    }

    // 함수를 코루틴으로 실행
    public void StartFunctionForCoroutine(Action func)
    {
        // 코루틴을 실행
        _coroutine = StartCoroutine(FunctionForCoroutine(func));
    }

    // 상태 변경
    public void ChangeState(int state)
    {
        State changeState = (State)state;
        _currentState = changeState;
    }

    // 상태를 다음 상태로 토글한다.
    public void ToggleState()
    {
        // State enum의 값을 배열로 변환
        State[] enumValues = (State[])Enum.GetValues(typeof(State));

        // 현재 값의 인덱스 찾기
        int currentIndex = Array.IndexOf(enumValues, _currentState);

        // 다음 State 값으로 전환
        State changeState = enumValues[(currentIndex + 1) % enumValues.Length];

        _currentState = changeState;
    }


    /*************************************************
     *               Private Methods
     *************************************************/
    // 함수를 코루틴으로 실행
    private IEnumerator FunctionForCoroutine(Action func)
    {
        // 코루틴이 실행중이면 즉시 종료
        if (IsCoroutineRunning()) { yield break; }

        // 대기 & 함수 실행 & 코루틴 초기화
        yield return _delay;
        func();
        ResetCoroutine();
    }

    // 코루틴이 실행중인지 검사
    private bool IsCoroutineRunning()
    {
        return _coroutine != null;
    }

    // 코루틴을 초기화
    private void ResetCoroutine()
    {
        _coroutine = null;
    }
}
