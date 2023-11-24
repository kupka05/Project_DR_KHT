using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenText : MonoBehaviour
{
    public TMP_Text text;
    public CanvasGroup canvasGroup;
    IEnumerator fadeRoutine;

    [Tooltip("How fast to fade in / out")]
    public float FadeInSpeed = 6f;

    public float FadeOutSpeed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnScreenText(string newText)
    {
        if(!canvasGroup.gameObject)
        {
            canvasGroup.gameObject.SetActive(true);
        }
        text.text = newText;
        DoFadeIn();
    }

    public virtual void DoFadeIn()
    {
        // Stop if currently running
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        // Do the fade routine
        if (canvasGroup != null)
        {
            fadeRoutine = doFade(canvasGroup.alpha, 1);
            StartCoroutine(fadeRoutine);
        }
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
}
