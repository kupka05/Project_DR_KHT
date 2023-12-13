using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNpc : AnnouncementNPC
{       // 상점 NPC의 Class

    

    protected override void Awake()
    {
        base.Awake();
        AwakeInIt();
    }       // Awake()


    private void Start()
    {

    }       // Start()


    private void AwakeInIt()
    {
        npcID = 11_1_12_01;
        
    }       // AwakeInIt()




}       // ClassEnd
