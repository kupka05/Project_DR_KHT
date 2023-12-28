using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterDeadCheck : MonoBehaviour
{
    private BossRoom bossRoom;

    public void InItBossRoom(BossRoom _bossRoom)
    {
        this.bossRoom = _bossRoom;

        bossRoom.bossList.Add(this.gameObject);
    }

    private void OnDestroy()
    {
        bossRoom.bossList.Remove(this.gameObject);
        bossRoom.CheckClearBoss();
    }

    public void BossDie()
    {
        bossRoom.bossList.Remove(this.gameObject);
        bossRoom.CheckClearBoss();
    }

}
