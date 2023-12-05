using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyEvent : MonoBehaviour
{
    [Header("Door")]
    public GameObject spawnroomDoor;
    public GameObject openDoorButton;


    [Header("Display")]
    public GameObject mainDisplay;
    public GameObject mbtiDisplay;

    public void Start()
    {
        ChangeDisplayButton("Main");
    }


    public void ChangeDisplayButton(string name)
    {
        mainDisplay.SetActive(false);
        mbtiDisplay.SetActive(false);

        switch (name)
        {
            case "Main":
                mainDisplay.SetActive(true);
                break;
            case "MBTI":
                mbtiDisplay.SetActive(true);
                break;
        }

    }

    public void OpenSpawnRoomDoor()
    {
        spawnroomDoor.GetComponent<Animation>().Play("SpawnRoom_Open");
        openDoorButton.SetActive(false);
    }
}
