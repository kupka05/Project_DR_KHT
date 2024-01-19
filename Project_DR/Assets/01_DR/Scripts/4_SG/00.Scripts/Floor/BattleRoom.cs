using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BattleRoom : RandomRoom
{       // BattleRoomClass는 Monster소환 Monster가 전부 죽었는지 체크할것

    public List<GameObject> monsterList; // 소환한 몬스터를 관리해줄 List
    private List<Vector3> spawnPointList;      // 소환된 몬스터의 위치를 관리해줄 List
    private GameObject spawnMonster;     // 몬스터 스폰시 사용될 GameObject
    private GameObject monstersParent;          // 하이얼하키창에서 몬스터를 담아줄 parentObj
    private StringBuilder stringBuilder;        // 변화하는 string사용을 최저한 StringBuilder

    private int recallCount; // 재귀를 몇번했는지 체크할 함수
    private int maxRecallCount; // 최대재귀횟수

    private bool isSoundPlay = default;   // 전투방 음악 재생을 했는지 체크할 변수
    private string playSoundName = default;
    private string inStageSoundName = default;

    void Start()
    {
        GameManager.isClearRoomList.Add(this);
        StartCoroutine(StartMethodDelay());

    }       // Start()

    private void OnDestroy()
    {
        GameManager.isClearRoomList.Remove(this);
        StopAllCoroutines();        // 예의치 못한 코루틴 으로 인한 이슈 방지
    }       // OnDestroy()

    /// <summary>
    /// 몬스터 몇마리를 스폰할지 정하는 함수
    /// </summary>
    private void SettingSpawnCount()
    {
        // 아래 int변수들은 스프레드시트에 적힌 값중에서 랜덤하게 적용될 변수
        int nomalSpawnCount = 0;
        int eliteSpawnCount = 0;
        int obstacleSpawnCount = 0;

        if (GameManager.instance.nowFloor == 1)
        {
            nomalSpawnCount = Random.Range((int)DataManager.Instance.GetData(11001, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(11001, "MaxValue", typeof(int)));
            eliteSpawnCount = Random.Range((int)DataManager.Instance.GetData(11002, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(11002, "MaxValue", typeof(int)));
            obstacleSpawnCount = Random.Range((int)DataManager.Instance.GetData(11003, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(11003, "MaxValue", typeof(int)));
        }
        else if (GameManager.instance.nowFloor == 2)
        {
            nomalSpawnCount = Random.Range((int)DataManager.Instance.GetData(12001, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(12001, "MaxValue", typeof(int)));
            eliteSpawnCount = Random.Range((int)DataManager.Instance.GetData(12002, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(12002, "MaxValue", typeof(int)));
            obstacleSpawnCount = Random.Range((int)DataManager.Instance.GetData(12003, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(12003, "MaxValue", typeof(int)));
        }
        else if (GameManager.instance.nowFloor == 3)
        {
            nomalSpawnCount = Random.Range((int)DataManager.Instance.GetData(13001, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(13001, "MaxValue", typeof(int)));
            eliteSpawnCount = Random.Range((int)DataManager.Instance.GetData(13002, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(13002, "MaxValue", typeof(int)));
            obstacleSpawnCount = Random.Range((int)DataManager.Instance.GetData(13003, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(13003, "MaxValue", typeof(int)));
        }
        else if (GameManager.instance.nowFloor == 4)
        {
            nomalSpawnCount = Random.Range((int)DataManager.Instance.GetData(14001, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(14001, "MaxValue", typeof(int)));
            eliteSpawnCount = Random.Range((int)DataManager.Instance.GetData(14002, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(14002, "MaxValue", typeof(int)));
            obstacleSpawnCount = Random.Range((int)DataManager.Instance.GetData(14003, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(14003, "MaxValue", typeof(int)));
        }
        else if (GameManager.instance.nowFloor == 5)
        {
            nomalSpawnCount = Random.Range((int)DataManager.Instance.GetData(15001, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(15001, "MaxValue", typeof(int)));
            eliteSpawnCount = Random.Range((int)DataManager.Instance.GetData(15002, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(15002, "MaxValue", typeof(int)));
            obstacleSpawnCount = Random.Range((int)DataManager.Instance.GetData(15003, "MinValue", typeof(int)),
                    1 + (int)DataManager.Instance.GetData(15003, "MaxValue", typeof(int)));
        }
        else { GFunc.LogErrorFormat("들어오면 안되는 곳에 들어옴"); }

        SettingSpawnMonster(nomalSpawnCount, eliteSpawnCount, obstacleSpawnCount);

    }       // SettingSpon()

    /// <summary>
    /// 어떤 몬스터를 소폰할지 정하는 함수
    /// </summary>
    /// <param name="_nomalSpawnCount">노말 몬스터 소폰될 수</param>
    /// <param name="_eliteSpawnCount">엘리트 몬스터 소폰될 수</param>
    /// <param name="_obstacleSpawnCount">장애물 스폰될 수</param>
    private void SettingSpawnMonster(int _nomalSpawnCount, int _eliteSpawnCount, int _obstacleSpawnCount)
    {
        int defaultObjectVariety;   // 기본값 + 종류의 값으로 어떤 리소스이름인지 검출할것임
        int objectVariety = 0;      // 오브젝트 종류가 몇개있는지 담아둘 변수

        defaultObjectVariety = 10000;
        objectVariety = DataManager.Instance.GetCount(10001);
        for (int nomalMonster = 0; nomalMonster < _nomalSpawnCount; nomalMonster++)
        {       // 노말 몬스터 

            // 어떤 몬스터할지
            int randomNum = Random.Range(1, objectVariety + 1);
            int reslutNum = randomNum + defaultObjectVariety;
            stringBuilder.Clear();
            //stringBuilder.Append(GameManager.instance.nomalMonsterSpawnPath);
            stringBuilder.Append((string)DataManager.Instance.GetData(reslutNum, "ResourceName", typeof(string)));

            // 어느 위치에 소환할지
            Vector3 spawnPoint = SettingSpawnPoint();
            spawnPointList.Add(spawnPoint);

            // 소환
            SpawnMonster(spawnPoint);
            //GFunc.Log($"{gameObject.name} : {stringBuilder}");
        }

        int defaultEliteMonsterID = 10006;
        objectVariety = Data.GetCount(defaultEliteMonsterID + 1);
        //GFunc.Log($"GetCount해온값 : {objectVariety}");
        for (int nowEliteMonsterSpawnCount = 0; nowEliteMonsterSpawnCount < _eliteSpawnCount; nowEliteMonsterSpawnCount++)
        {
            int randNum = Random.Range(0, objectVariety + 1);
            int reslutNum = randNum + defaultEliteMonsterID;
            stringBuilder.Clear();
            stringBuilder.Append(Data.GetString(reslutNum, "ResourceName"));

            Vector3 spawnPoint = SettingSpawnPoint();
            spawnPointList.Add(spawnPoint);

            // 소환
            SpawnMonster(spawnPoint);
        }

    }       // ChoiceSpawnCount()

    private Vector3 SettingSpawnPoint()
    {

        float spawnPointX = Random.Range(meshPos.bottomLeftCorner.x + 3f, meshPos.bottomRightCorner.x - 3f);
        float spawnPointY = 1f;
        float spawnPointZ = Random.Range(meshPos.bottomLeftCorner.z + 3f, meshPos.topLeftCorner.z - 3f);
        Vector3 spawnPoint = new Vector3(spawnPointX, spawnPointY, spawnPointZ);
        bool isExamineSpawnPosition = ExamineSpawnPosition(spawnPoint);
        if (isExamineSpawnPosition == true && recallCount < maxRecallCount)
        {
            recallCount++;
            SettingSpawnPoint();
        }

        return spawnPoint;
    }

    /// <summary>
    /// 인스턴스전 곂치는 Pos가 있는지 확인하는 함수
    /// </summary>
    /// <param name="spawnPoint_">현재 랜덤으로 나온 포지션</param>
    /// <returns></returns>
    private bool ExamineSpawnPosition(Vector3 spawnPoint_)
    {
        bool examineReslut = false;

        foreach (Vector3 existingXPos in spawnPointList)
        {
            //if(existingPos.x == spawnPoint_.x || existingPos.z == spawnPoint_.z)            {           }
            float posDis = Vector3.Distance(existingXPos, spawnPoint_);
            if (posDis < 2)
            {
                examineReslut = true;
                return examineReslut;
            }
        }

        return examineReslut;
    }       // ExamineSpawnPosition()



    /// <summary>
    /// 몬스터를 스폰하는 함수
    /// </summary>
    /// <param name="_spawnPoint">스폰할 몬스터의 좌표</param>    
    private void SpawnMonster(Vector3 _spawnPoint)
    {
        if (recallCount >= maxRecallCount)
        {
            recallCount = 0;
            return;
        }
        GameObject prefabObj = Resources.Load<GameObject>($"{stringBuilder}");
        //GFunc.Log($"GameObjectIsNull? : {prefabObj == null} , SB : {stringBuilder}");
        spawnMonster = Instantiate(prefabObj, _spawnPoint, Quaternion.identity, monstersParent.transform);

        //GFunc.Log($"소환된 몬스터 이름 : {spawnMonster.gameObject.name}");


        SpawnMonsterSetting(spawnMonster);


    }       // SponMonster()


    /// <summary>
    /// 스폰한 몬스터에 자신의 스크립트를 참조하게 하고 List에 자신을 넣게 해주는 함수
    /// </summary>
    /// <param name="spawnMonster"></param>
    private void SpawnMonsterSetting(GameObject spawnMonster)
    {
        spawnMonster.AddComponent<MonsterDeadCheck>();
        MonsterDeadCheck monsterDeadCheck = spawnMonster.GetComponent<MonsterDeadCheck>();

        monsterDeadCheck.BattleRoomInIt(this);
        monsterDeadCheck.AddList();
        // ! 몬스터가 OnDestroy될떄에 List에서 자기자신을 삭제할거임

    }       // SpawnMonsterSetting()

    /// <summary>
    /// Start함수에서 딜레이 준뒤에 몬스터 셋팅 시작하도록할 코루틴
    /// </summary>    
    IEnumerator StartMethodDelay()
    {
        GameManager.instance.spawnDelayFrame++;
        for (int i = 0; i < GameManager.instance.spawnDelayFrame; i++)
        {
            yield return null;
        }


        FirstSetting();         // Start에서 초기화 할당 매서드

        SettingSpawnCount();    // 여기서 부터 몬스터 생성 시작

    }       // C_StartMethodDelay()

    /// <summary>
    /// 코루틴 딜레이 이후에 Start함수 처럼 사용될 함수
    /// </summary>
    private void FirstSetting()
    {
        GetFloorPos();      // 꼭지점 가져와주는 Class
        stringBuilder = new StringBuilder();
        monsterList = new List<GameObject>();
        spawnPointList = new List<Vector3>();
        monstersParent = new GameObject("Monsters");
        monstersParent.transform.parent = this.transform;
        recallCount = 0;
        maxRecallCount = 5;
        isSoundPlay = false;
        playSoundName = "BGM_Stage_Battle";
        inStageSoundName = "BGM_Stage_InStage";

    }       // FirstSetting()


    /// <summary>
    /// 해당 방이 클리어 됬는지 체크할 함수(List<GameObject> 의 count == 0일때 클리어)
    /// </summary>
    public void CheckClearRoom()
    {
        if (monsterList.Count == 0)
        {
            ClearRoomBoolSetTrue();
            InStageSoundPlay();
        }
        else { /*PASS*/ }

    }       // CheckClearRoom()

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.transform.CompareTag("Player"))
        {
            BattleRoomSoundPlay();
        }
        else { /*PASS*/ }

    }       // OnCollisionEnter()

    /// <summary>
    /// 전투방 입장시 한번만 사운드 플레이해주는 함수
    /// </summary>
    private void BattleRoomSoundPlay()
    {
        if (isSoundPlay == false)
        {
            isSoundPlay = true;
            AudioManager.Instance.AddBGM(playSoundName);
            AudioManager.Instance.PlayBGM(playSoundName);
        }

        else { /*PASS*/ }
    }       // BattleRoomSoundPlay()

    private void InStageSoundPlay()
    {
        AudioManager.Instance.PlayBGM(inStageSoundName);
    }



}       // ClassEnd
