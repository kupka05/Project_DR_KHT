using BNG;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTeleport : MonoBehaviour
{
    public Transform spawnPosition;
    private ScreenFader fader;
    private GameObject player;
    private Vector3 position;

    private void Start()
    {
        fader = Camera.main.GetComponentInChildren<ScreenFader>();
        position = spawnPosition.position;
        position.y += 1f;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            fader.DoFadeIn();
            Invoke(nameof(FadeOut), 1f);
        }
    }

    private void FadeOut()
    {
        player.transform.position = position;
        fader.DoFadeOut();
    }
}
