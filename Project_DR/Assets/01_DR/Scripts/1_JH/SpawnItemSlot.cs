using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class SpawnItemSlot : MonoBehaviour
{
    private BoxCollider boxCollider;
    public GameObject curGrabber;
    
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Grabber>())
        {
            curGrabber = other.gameObject;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        curGrabber = null;
    }
}
