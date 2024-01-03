using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDoor : MonoBehaviour
{
    /*************************************************
     *                Private Fields
     *************************************************/
    [SerializeField] private Animation _animation;
    [SerializeField] private Collider _shield;
    [SerializeField] private float _delay = 2f;


    /*************************************************
     *                 Unity Events
     *************************************************/
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
            // 플레이어가 못나가게 문을 막음
            Invoke("ActiveShield", 0.5f);

            // n초 후 문을 닫는다.
            Invoke("CloseDoor", _delay);
        }
    }


    /*************************************************
     *             Initialize Methods
     *************************************************/
    public void Initialize()
    {
        // Init
        _animation = transform.parent.Find("Door_Middle").GetComponent<Animation>();
        _shield = transform.parent.Find("Shield").gameObject.GetComponent<BoxCollider>();
    }


    /*************************************************
     *              Private Methods
     *************************************************/
    private void CloseDoor()
    {
        _animation.Play("ShopDoor_Close");
        Destroy(this);
    }

    private void ActiveShield()
    {
        _shield.enabled = true;
    }
}
