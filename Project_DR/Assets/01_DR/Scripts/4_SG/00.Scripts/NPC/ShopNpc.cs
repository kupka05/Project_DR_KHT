using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNpc : AnnouncementNPC
{

    private void Awake()
    {
        AwakeInIt();
    }       // Awake()


    private void Start()
    {

    }       // Start()


    private void AwakeInIt()
    {
        animator = GetComponent<Animator>();
        dialogue = new System.Text.StringBuilder();
    }       // AwakeInIt()




}       // ClassEnd
