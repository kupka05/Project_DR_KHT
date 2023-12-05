using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
using Slider = UnityEngine.UI.Slider;

public class PlayerOption : MonoBehaviour
{
    [Header("Reference")]
    public GameObject player;
    public GameObject optionUI;      // 켜지고 꺼질 옵션의 UI
    private PlayerRotation rotation;

    [Header("Scene")]
    public string loginSceneName;    // 이동할 로그인씬 명

    [Header("TMP Text")]
    public TMP_Text rotationText;    // 회전속도 텍스트

    [Header("Brightness")]
    public Volume volume;
    private ColorAdjustments brightness;

    [Header("Slider")]
    public Slider masterSlider;      // 마스터 사운드 슬라이더
    public Slider backgroundSlider;  // 배경음 슬라이더
    public Slider soundEffectSlider; // 효과음 슬라이더
    public Slider brightSlider;      // 화면 밝기 슬라이더


    [Header("Input")]
    //[Tooltip("The key(s) to use to toggle locomotion type")]
    public List<ControllerBinding> optionInput = new List<ControllerBinding>() { ControllerBinding.None };
    public InputActionReference InputAction = default;

    private void Start()
    {
        rotation = player.GetComponent<PlayerRotation>();
        volume.profile.TryGet<ColorAdjustments>(out brightness);
        if (brightness == null)
            Debug.LogError("볼륨을 찾을 수 없음");
    }

    public void Update()
    {
    }

    // 환경설정 UI 입력
    public virtual void CheckOptionToggleInput()
    {
        // Check for bound controller button
        for (int x = 0; x < optionInput.Count; x++)
        {
            if (InputBridge.Instance.GetControllerBindingValue(optionInput[x]))
            {
                if (optionUI)
                {
                    optionUI.SetActive(!optionUI.activeSelf);
                }
            }
        }
    }

    // 환경설정 토글
    private void OnEnable()
    {
        InputAction.action.performed += ToggleActive;
    }

    private void OnDisable()
    {
        InputAction.action.performed -= ToggleActive;
    }

    public void ToggleActive(InputAction.CallbackContext context)
    {
        if (optionUI)
        {
            optionUI.SetActive(!optionUI.activeSelf);
        }
    }

    // 회전 속도 변경
    public void SetRotation()
    {
        rotation.SetRotation();
        rotationText.text = string.Format(rotation.SnapRotationAmount + "˚");
    }

    // 메인으로 이동
    public void GotoMain()
    {
        SceneManager.LoadScene(loginSceneName);
    }
    // 게임 종료
    public void QuitGame()
    {
        Application.Quit();
    }

    // 화면 밝기 조정
    public void BrightnessSlider(float value)
    {
        brightness.postExposure.value= value;
    }

    // 마스터 조절 버튼
    public void SetMasterButton(float value)
    {
        float newValue = ValueCheck(masterSlider.value, value);
        masterSlider.value = newValue;
        SetMasterSlider(newValue);
    }
    // 배경음 조절 버튼
    public void SetBackgroundButton(float value)
    {
        float newValue = ValueCheck(backgroundSlider.value, value);
        backgroundSlider.value = newValue;
        SetBackGroundSlider(newValue);
    }
    // 사운드 이펙트 조절 버튼
    public void SetSoundEffectButton(float value)
    {
        float newValue = ValueCheck(soundEffectSlider.value, value);
        soundEffectSlider.value = newValue;
        SetSoundEffectSlider(newValue);
    }

    // 마스터 사운드 조정
    public void SetMasterSlider(float value)
    {
        //ToDo: 마스터 사운드 연동 필요
    }
    // 배경음 조정
    public void SetBackGroundSlider(float value)
    {
        //ToDo: 효과음 연동 필요
    }

    // 효과음 조정 
    public void SetSoundEffectSlider(float value)
    {
        //ToDo: 효과음 연동 필요
    }

    // 값 체크
    float ValueCheck(float slider, float value)
    {
        float newValue = slider + value;
        if (100 < newValue)
            newValue = 100;
        else if (newValue < 0)
            newValue = 0;
        return newValue;
    }
}
