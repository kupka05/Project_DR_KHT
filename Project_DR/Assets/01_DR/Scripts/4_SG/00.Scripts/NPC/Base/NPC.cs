using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NPC : MonoBehaviour
{    
    protected Animator animator;        // Npc 애니메이터
    protected StringBuilder dialogue;   // Npc 대사 출력해줄 Sb
    protected string npcName;           // Npc 이름
    protected string npcTitle;          // Npc 칭호
    protected int npcID;                // NPC 스프레드시트 ID

    protected virtual void Awake()
    {
        AwakeInIt();
    }

    private void AwakeInIt()
    {
        animator = GetComponent<Animator>();
        dialogue = new StringBuilder();
    }
}           // ClassEnd
