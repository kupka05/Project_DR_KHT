using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine.UI;

public class TutorialCutScene : MonoBehaviour
{
    [Header("Spawn Position")]
    public Transform SpawnPosition;


    [Header("Dialogs")]
    public string[] dialogs =
    {
        "!!!! System : 탐험자'에게 치명적인 위험 발생 !!!!",
        "의식을 잃은 탐험자의 신변 보호를 위하여",
        "훈련 차원으로 강제 이동합니다."
    };
    public float dialogTime;
    public float dialogDelay;

    private GameObject player;
    private ScreenFader fader;
    private TMP_Text dialogText;
    WaitForSeconds waitForSeconds;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fader = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenFader>();
        dialogText = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        waitForSeconds = new WaitForSeconds(dialogTime);
        Invoke(nameof(StartCutScene), 1f);
    }

    void StartCutScene()
    {
        fader.SetFadeColor(Color.white);
        StartCoroutine(Dialog());
    }
    IEnumerator Dialog()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < dialogs.Length; i++) 
        {
            dialogText.DOText(dialogs[i], dialogDelay);
            yield return waitForSeconds;
            dialogText.text = null;
        }
        yield return waitForSeconds;

        fader.DoFadeOut();
        StartTutorial();
        yield return new WaitForSeconds(1f);

        fader.SetFadeColor(Color.black);
        yield break;
    }



    void StartTutorial()
    {
        player.transform.position = SpawnPosition.position;
        player.transform.rotation = SpawnPosition.rotation;
    }


}
