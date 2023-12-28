using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPoolManager;

public class ObjectPoolManager : MonoBehaviour
{
    // 프로젝타일의 종류를 나타내는 열거형
    public enum ProjectileType
    {
        CHAINBULLET,
        BOUNCEBALL,
        BOUNCEBULLET,
        BIGBRICK
    }

    // 오브젝트 풀을 초기화할 때 사용할 프리팹들
    public GameObject objectPrefeb;
    public GameObject bouncePrefab;
    public GameObject bounceSmallPrefab;
    public GameObject brickPrefab;

    // 오브젝트를 담을 큐
    Queue<GameObject> ObjectPool = new Queue<GameObject>();

    // 오브젝트 풀 매니저의 싱글톤 인스턴스
    public static ObjectPoolManager instance = null;

    // 게임 시작 시 호출되는 메서드
    void Start()
    {
        InitializeObjectPool();
        Debug.Log("오브젝트 풀 매니저가 시작되었습니다.");
    }

    // 오브젝트 풀을 초기화하는 메서드
    void InitializeObjectPool()
    {
        // 인스턴스가 없다면 현재 인스턴스를 설정하고, 씬 전환 시 파괴되지 않도록 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // 각각의 프로젝타일 종류에 대해 오브젝트를 생성하여 풀에 추가
            CreateObjects(ProjectileType.CHAINBULLET);
            CreateObjects(ProjectileType.BOUNCEBALL);
            CreateObjects(ProjectileType.BOUNCEBULLET);
            CreateObjects(ProjectileType.BIGBRICK);
        }
        else
        {
            // 이미 인스턴스가 있다면 현재 게임 오브젝트를 파괴
            Destroy(this.gameObject);
            Debug.LogWarning("오브젝트 풀 매니저가 이미 존재하므로 중복된 것을 파괴합니다.");
        }
    }

    // 오브젝트를 생성하여 풀에 추가하는 메서드
    void CreateObjects(ProjectileType projectileType)
    {
        for (int i = 0; i < 300; i++)
        {
            CreateObject(projectileType);
            Debug.Log($"프로젝타일 타입 {projectileType}을(를) 생성했습니다. (총 {i + 1}개)");
        }
    }

    // 특정 프로젝타일에 대한 오브젝트를 생성하고 풀에 추가하는 메서드
    GameObject CreateObject(ProjectileType projectileType)
    {
        // 주어진 프로젝타일에 해당하는 프리팹 가져오기
        GameObject prefab = GetPrefabType(projectileType);

        if (prefab == null)
        {
            // 프리팹이 null이면 오류 출력 후 메서드 종료
            Debug.LogError("프리팹이 null입니다. ProjectileType: " + projectileType);
            return null;
        }

        // 프리팹을 복제하여 새로운 오브젝트 생성 및 풀에 추가
        GameObject newObj = Instantiate(prefab, instance.transform);
        newObj.SetActive(false);

        Debug.Log($"프로젝타일 타입 {projectileType}을(를) 풀에 추가하고 비활성화했습니다.");

        return newObj;
    }

    // 특정 프로젝타일에 대한 프리팹을 반환하는 메서드
    GameObject GetPrefabType(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.CHAINBULLET:
                return instance.objectPrefeb;

            case ProjectileType.BOUNCEBALL:
                return instance.bouncePrefab;

            case ProjectileType.BOUNCEBULLET:
                return instance.bounceSmallPrefab;

            case ProjectileType.BIGBRICK:
                return instance.brickPrefab;

            default:
                return null;
        }
    }

    // 특정 프로젝타일에 대한 오브젝트를 풀에서 가져오는 메서드
    public static GameObject GetObject(ProjectileType projectileType)
    {
        if (instance.ObjectPool.Count > 0)
        {
            // 풀에서 오브젝트를 꺼내와서 활성화하고 부모를 해제
            GameObject objectInPool = instance.ObjectPool.Dequeue();
            objectInPool.SetActive(true);
            GFunc.Log($"{projectileType}을(를) 풀에서 가져와 활성화했습니다.");
            objectInPool.transform.SetParent(null);
            return objectInPool;
        }
        else
        {
            // 풀이 비어있다면 새로운 오브젝트를 생성하여 활성화하고 부모를 해제
            GameObject objectInPool = instance.CreateObject(projectileType);
            objectInPool.SetActive(true);
            Debug.Log($"{projectileType}을(를) 풀에서 가져오지 못해 새로 생성하고 활성화했습니다.");
            objectInPool.transform.SetParent(null);
            return objectInPool;
        }
    }

    // 오브젝트를 풀에 반환하는 메서드
    public static void ReturnObjectToQueue(GameObject obj, ProjectileType projectileType)
    {
        if (obj != null && obj.activeSelf)
        {
            // 오브젝트 초기화 후 비활성화하고 부모를 설정하여 풀에 추가
            ResetObject(obj, projectileType);
            obj.SetActive(false);
            GFunc.Log($"프로젝타일을 풀에 반환했습니다.");
            obj.transform.SetParent(instance.transform);
            instance.ObjectPool.Enqueue(obj);
        }
    }

    // 오브젝트를 초기 상태로 되돌리는 메서드
    private static void ResetObject(GameObject obj, ProjectileType projectileType)
    {
        switch(projectileType)
        {
            case ProjectileType.CHAINBULLET:
                Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
                if(rigidbody != null)
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                }
                break;
            case ProjectileType.BOUNCEBALL:
                Rigidbody rigidbodyBounce = obj.GetComponent<Rigidbody>();
                if(rigidbodyBounce != null)
                {
                    rigidbodyBounce.velocity = Vector3.zero;
                    rigidbodyBounce.angularVelocity = Vector3.zero;
                }
                //SphereCollider sphereCollider = obj.GetComponent<SphereCollider>();
                //if(sphereCollider != null)
                //{
                //    sphereCollider.isTrigger = false;
                //}
                
                break;

            case ProjectileType.BOUNCEBULLET:
                Rigidbody rigidbodyBullet = obj.GetComponent<Rigidbody>();
                if(rigidbodyBullet != null)
                {
                    rigidbodyBullet.velocity = Vector3.zero;
                    rigidbodyBullet.angularVelocity = Vector3.zero;
                }
                break;

            case ProjectileType.BIGBRICK:
                Rigidbody rigidbodyBrick = obj.GetComponent<Rigidbody>();
                if (rigidbodyBrick != null)
                {
                    rigidbodyBrick.velocity = Vector3.zero;
                    rigidbodyBrick.angularVelocity = Vector3.zero;
                    rigidbodyBrick.useGravity = false;
                }
                SphereCollider sphereColliderBrick = obj.GetComponent<SphereCollider>();
                if(sphereColliderBrick != null)
                {
                    sphereColliderBrick.isTrigger = false;
                }
                break;
        }
    }


        //Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
        //if (rigidbody != null)
        //{
        //    rigidbody.velocity = Vector3.zero;
        //    rigidbody.angularVelocity = Vector3.zero;
        //}

        //Collider collider = obj.GetComponent<Collider>();
        //if (collider != null)
        //{
        //    collider.enabled = true;

        //    // ProjectileType에 따라 초기화 설정
        //    switch (projectileType)
        //    {
        //        case ProjectileType.BOUNCEBALL:
        //            SphereCollider sphereCollider = obj.GetComponent<SphereCollider>();
        //            if (sphereCollider == null)
        //            {
        //                // SphereCollider가 없으면 추가
        //                sphereCollider = obj.AddComponent<SphereCollider>();
        //            }
        //            // BOUNCEBALL에 대한 초기화 설정
        //            sphereCollider.isTrigger = true;
        //            // 필요한 다른 초기화 설정...
        //            break;

        //    }
        //}
    
}


