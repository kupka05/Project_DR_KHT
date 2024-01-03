using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDoor : MonoBehaviour
{
    private Animation _animation;

    void Start()
    {
        // Init
        Initialize();
    }

    // 상점 안에 들어올 경우 문을 닫는다.
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 들어왔을 경우
        if (other.CompareTag("Player"))
        {
            // 1초 후 문을 닫는다.
            Invoke("CloseDoor", 1f);
        }
    }

    public void Initialize()
    {
        // Init
        _animation = transform.parent.Find("Door_Middle").GetComponent<Animation>();
    }

    private void CloseDoor()
    {
        _animation.Play("ShopDoor_Close");
        Destroy(this);
    }
}
