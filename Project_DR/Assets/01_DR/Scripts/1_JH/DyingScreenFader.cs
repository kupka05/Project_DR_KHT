using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DyingScreenFader : MonoBehaviour
{
    [Tooltip("Should the screen fade in when a new level is loaded")]
    public bool FadeOnSceneLoaded = true;

    [Tooltip("Color of the fade. Alpha will be modified when fading in / out")]
    public Color FadeColor = Color.black;

    [Tooltip("How fast to fade in / out")]
    public float FadeInSpeed = 6f;

    public float FadeOutSpeed = 6f;

    [Tooltip("Wait X seconds before fading scene in")]
    public float SceneFadeInDelay = 1f;

    GameObject fadeObject;
    RectTransform fadeObjectRect;
    Canvas fadeCanvas;
    CanvasGroup canvasGroup;
    Image fadeImage;
    IEnumerator fadeRoutine;
    public string dyingFaderName = "DyingScreenFader";

    public Sprite dyingScreen;

    void Awake()
    {
        initialize();
    }
   
    protected virtual void initialize()
    {
        // Create a Canvas that will be placed directly over the camera
        if (fadeObject == null)
        {
            CreateFader(dyingFaderName);
        }
    }
    private void CreateFader(string faderName)
    {
        Canvas childCanvas = GetComponentInChildren<Canvas>();

        // Found existing item, no need to initialize this one
        if (childCanvas != null && childCanvas.transform.name == faderName)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        fadeObject = new GameObject();
        fadeObject.transform.parent = Camera.main.transform;
        fadeObject.transform.localPosition = new Vector3(0, 0, 0.03f);
        fadeObject.transform.localEulerAngles = Vector3.zero;
        fadeObject.transform.name = faderName;

        fadeCanvas = fadeObject.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.WorldSpace;
        fadeCanvas.sortingOrder = 100; // Make sure the canvas renders on top

        canvasGroup = fadeObject.AddComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;

        fadeImage = fadeObject.AddComponent<Image>();
        fadeImage.color = FadeColor;
        fadeImage.sprite = dyingScreen;
        fadeImage.raycastTarget = false;

        // Stretch the image
        fadeObjectRect = fadeObject.GetComponent<RectTransform>();
        fadeObjectRect.anchorMin = new Vector2(1, 0);
        fadeObjectRect.anchorMax = new Vector2(0, 1);
        fadeObjectRect.pivot = new Vector2(0.5f, 0.5f);
        fadeObjectRect.sizeDelta = new Vector2(0.07f, 0.04f);
        fadeObjectRect.localScale = new Vector2(2f, 2f);
    }

    private void Start()
    {
        GetData();
    }
 
    public void OnDamage()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        canvasGroup.alpha = 0.5f;
        fadeRoutine = doFade(canvasGroup.alpha, 0);
        StartCoroutine(fadeRoutine);
    }


    IEnumerator doFade(float alphaFrom, float alphaTo)
    {

        float alpha = alphaFrom;

        updateImageAlpha(alpha);

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

            updateImageAlpha(alpha);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();

        // Ensure alpha is always applied
        updateImageAlpha(alphaTo);
    }

    protected virtual void updateImageAlpha(float alphaValue)
    {

        // Canvas Group was Destroyed.
        if (canvasGroup == null)
        {
            return;
        }

        // Enable canvas if necessary
        if (!canvasGroup.gameObject.activeSelf)
        {
            canvasGroup.gameObject.SetActive(true);
        }

        canvasGroup.alpha = alphaValue;

        // Disable Canvas if we're done
        if (alphaValue == 0 && canvasGroup.gameObject.activeSelf)
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }
    private void GetData()
    {
        FadeInSpeed = (float)DataManager.GetData(1001, "FadeSpeed");
        FadeOutSpeed = (float)DataManager.GetData(1001, "FadeSpeed");

    }
}
