using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DamageScreenFader : MonoBehaviour
{
    [Tooltip("Color of the fade. Alpha will be modified when fading in / out")]
    public Color FadeColor = Color.white;

    [Tooltip("How fast to fade in / out")]
    public float FadeInSpeed = 6f;

    public float FadeOutSpeed = 6f;

    [Tooltip("Wait X seconds before fading scene in")]
    public float SceneFadeInDelay = 1f;

    public float blinkSpeed = 0.2f;

    CanvasGroup damageCanvasGroup;
    CanvasGroup dyingCanvasGroup;
    IEnumerator damageRoutine;
    IEnumerator dyingRoutine;
    public string damageFaderName = "DamageScreenFader";
    public string dyingFaderName = "DyingScreenFader";

    public Sprite damageScreen;

    void Awake()
    {
        initialize();
    }
   
    protected virtual void initialize()
    {
        // Create a Canvas that will be placed directly over the camera
        if (damageCanvasGroup == null)
        {
            damageCanvasGroup = CreateFader(damageFaderName);
        }
        if (dyingCanvasGroup == null)
        {
            dyingCanvasGroup = CreateFader(dyingFaderName);
        }
    }
    private CanvasGroup CreateFader(string faderName)
    {
        Canvas childCanvas = GetComponentInChildren<Canvas>();

        // Found existing item, no need to initialize this one
        if (childCanvas != null && childCanvas.transform.name == faderName)
        {
            GameObject.Destroy(this.gameObject);
            return null;
        }
        GameObject damageFadeObject = new GameObject();
        damageFadeObject.transform.parent = Camera.main.transform;
        damageFadeObject.transform.localPosition = new Vector3(0, 0, 0.03f);
        damageFadeObject.transform.localEulerAngles = Vector3.zero;
        damageFadeObject.transform.name = faderName;

        Canvas fadeCanvas = damageFadeObject.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.WorldSpace;
        fadeCanvas.sortingOrder = 100; // Make sure the canvas renders on top

        CanvasGroup canvasGroup = damageFadeObject.AddComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;

        Image fadeImage = damageFadeObject.AddComponent<Image>();
        fadeImage.color = FadeColor;
        fadeImage.sprite = damageScreen;
        fadeImage.raycastTarget = false;

        // Stretch the image
        RectTransform fadeObjectRect = damageFadeObject.GetComponent<RectTransform>();
        fadeObjectRect.anchorMin = new Vector2(1, 0);
        fadeObjectRect.anchorMax = new Vector2(0, 1);
        fadeObjectRect.pivot = new Vector2(0.5f, 0.5f);
        fadeObjectRect.sizeDelta = new Vector2(0.07f, 0.04f);
        fadeObjectRect.localScale = new Vector2(2f, 2f);

        return canvasGroup;
    }

    private void Start()
    {
        GetData();
    }
 
    public void OnDamage()
    {
        if (damageRoutine != null)
        {
            StopCoroutine(damageRoutine);
        }
        damageCanvasGroup.alpha = 0.5f;
        damageRoutine = doFade(damageCanvasGroup, 0);
        StartCoroutine(damageRoutine);
    }

    public void OnDying()
    {
        if (dyingRoutine != null)
        {
            StopCoroutine(dyingRoutine);
        }
        dyingCanvasGroup.gameObject.SetActive(true);
        dyingRoutine = doBlink(blinkSpeed, 0.5f);
        StartCoroutine(dyingRoutine);
    }

    public void OnRestore()
    {
        if (dyingRoutine != null)
        {
            StopCoroutine(dyingRoutine);
            dyingCanvasGroup.gameObject.SetActive(false);
        }
    }

    IEnumerator doBlink(float speed,float maxAlpha)
    {
        while (true)
        {
            dyingCanvasGroup.alpha = Mathf.PingPong(Time.time*speed, 5)/10+ 0.1f;
            yield return null;
        }
    }

    IEnumerator doFade(CanvasGroup _canvasGroup, float alphaTo)
    {
        float alphaFrom = _canvasGroup.alpha;
        float alpha = alphaFrom;

        updateImageAlpha(alpha, _canvasGroup);

        while (alpha != alphaTo)
        {

            if (alphaFrom < alphaTo)
            {
                alpha += Time.deltaTime * FadeInSpeed;
                if (alpha > alphaTo)
                {
                    alpha = alphaTo;
                }
            }
            else
            {
                alpha -= Time.deltaTime * FadeOutSpeed;
                if (alpha < alphaTo)
                {
                    alpha = alphaTo;
                }
            }

            updateImageAlpha(alpha, _canvasGroup);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();

        // Ensure alpha is always applied
        updateImageAlpha(alphaTo, _canvasGroup);
    }

    protected virtual void updateImageAlpha(float alphaValue, CanvasGroup _canvasGroup)
    {

        // Canvas Group was Destroyed.
        if (_canvasGroup == null)
        {
            return;
        }

        // Enable canvas if necessary
        if (!_canvasGroup.gameObject.activeSelf)
        {
            _canvasGroup.gameObject.SetActive(true);
        }

        _canvasGroup.alpha = alphaValue;

        // Disable Canvas if we're done
        if (alphaValue == 0 && _canvasGroup.gameObject.activeSelf)
        {
            _canvasGroup.gameObject.SetActive(false);
        }
    }
    private void GetData()
    {
        FadeInSpeed = (float)DataManager.GetData(1001, "FadeSpeed", typeof(float));
        FadeOutSpeed = (float)DataManager.GetData(1001, "FadeSpeed", typeof(float));

    }
}
