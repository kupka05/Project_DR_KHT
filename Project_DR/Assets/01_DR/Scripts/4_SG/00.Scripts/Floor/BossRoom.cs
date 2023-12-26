using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    private GameObject spawnBossSkin;       // 보스의 외형 (분신)
    private GameObject spawnBossCapsule;    // 보스의 본체 캡슐

    private FloorMeshPos bossRoomPos;

    public List<GameObject> bossList;

    private void Awake()
    {
        bossList = new List<GameObject>();
    }

    public void VariablesInIt(FloorMeshPos _floorMeshPos, GameObject _spawnBossSkin, GameObject _spawnBossCapsule,
        Transform _dungeonFloor, Vector3 _centerPos)
    {
        StartCoroutine(BossInstanceCoroutine(_floorMeshPos, _spawnBossSkin, _spawnBossCapsule, _dungeonFloor, _centerPos));

    }

    IEnumerator BossInstanceCoroutine(FloorMeshPos _floorMeshPos, GameObject _spawnBossSkin, GameObject spawnBossCapsule,
            Transform _dungeonFloor, Vector3 _centerPos)
    {
        yield return new WaitForSeconds(5f);

        this.bossRoomPos = _floorMeshPos;
        GameObject monsterParent = new GameObject("BossMonster");
        monsterParent.transform.parent = _dungeonFloor.transform;

        GameObject bossClone;
        Vector3 bossPos = _centerPos;
        bossPos.y = bossPos.y + 3f;

        bossClone = Instantiate(_spawnBossSkin, bossPos, Quaternion.Euler(0f, 180f, 0f), monsterParent.transform);
        //bossClone.transform.localRotation = Quaternion.EulerRotation
        bossPos = _centerPos;
        bossPos.z = bossPos.z - 3f;
        bossPos.y = 1f;


        bossClone = Instantiate(spawnBossCapsule, bossPos, Quaternion.identity, monsterParent.transform);

        bossClone.AddComponent<BossMonsterDeadCheck>().InItBossRoom(this);

    }

    public void CheckClearBoss()
    {
        if(bossList.Count == 0)
        {
            GameManager.instance.IsBossRoomClear = true;
        }

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }



}
