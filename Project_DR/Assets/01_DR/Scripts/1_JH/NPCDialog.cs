using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// NPC 대사를 담을 클래스
[System.Serializable]
public class NpcDialogs
{
    public List<NpcDialog> logs;
}
[System.Serializable]
public class NpcDialog
{
    public int ID;
    public string[] _log;
    public Queue log;
    public int _event;
}