using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private SphereCollider grabberCollider;
    public GameObject inventory;

    private void Start()
    {
        grabberCollider = GetComponent<SphereCollider>();
    }

    // 인벤토리를 여는 이벤트
    public void OpenInventory()
    {
        inventory.SetActive(!inventory.activeSelf);
    }


}
