using BNG;
using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HumanTypeNPC : NPC
{       // 인간NPC의 BassClass


    protected override void Awake()
    {
        base.Awake();
        PointerSetting();
    }       // Awake()

    protected override void EndConveration()
    {
        ResetLayer();
    }       // EndConveration()


    // 포인터가 생기도록 초기화
    private void PointerSetting()
    {
        PointerEvents pointer = gameObject.AddComponent<PointerEvents>();
        pointer.MaxDistance = 1;
        int PhysicsPointerLayer = 16;
        this.gameObject.layer = PhysicsPointerLayer;
    }


    // 대화 종료 후 레이어 변경
    private void ResetLayer()
    {
        int DefaultLayer = 1;
        this.gameObject.layer = DefaultLayer;
    }


}
