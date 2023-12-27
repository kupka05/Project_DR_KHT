using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public enum ProjectileType
    {
        CHAINBULLET,
        BOUNCEBALL,
        BOUNCEBULLET,
        BIGBRICK
    }

    public GameObject objectPrefeb;
    public GameObject bouncePrefab;
    public GameObject bounceSmallPrefab;
    public GameObject brickPrefab;

    Queue<GameObject> ObjectPool = new Queue<GameObject>(); // 오브젝트를 담을 큐
    public static ObjectPoolManager instance = null;

    void Start()
    {
        InitializeObjectPool();
        Debug.Log("오브젝트 풀 매니저가 시작되었습니다.");
    }

    void InitializeObjectPool()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            CreateObjects(ProjectileType.CHAINBULLET, 100);
            CreateObjects(ProjectileType.BOUNCEBALL, 100);
            CreateObjects(ProjectileType.BOUNCEBULLET, 100);
            CreateObjects(ProjectileType.BIGBRICK, 100);
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("오브젝트 풀 매니저가 이미 존재하므로 중복된 것을 파괴합니다.");
        }
    }

    void CreateObjects(ProjectileType projectileType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateObject(projectileType);
            Debug.Log($"프로젝타일 타입 {projectileType}을(를) 생성했습니다. (총 {i + 1}개)");
        }
    }

    GameObject CreateObject(ProjectileType projectileType)
    {
        GameObject prefab = GetPrefabType(projectileType);

        if (prefab == null)
        {
            Debug.LogError("프리팹이 null입니다. ProjectileType: " + projectileType);
            return null;
        }

        GameObject newObj = Instantiate(prefab, instance.transform);
        newObj.SetActive(false);

        Debug.Log($"프로젝타일 타입 {projectileType}을(를) 풀에 추가하고 비활성화했습니다.");

        return newObj;
    }

    public static GameObject GetObject(ProjectileType projectileType)
    {
        if (instance.ObjectPool.Count > 0)
        {
            GameObject objectInPool = instance.ObjectPool.Dequeue();

            objectInPool.SetActive(true);
            GFunc.Log($"{projectileType}을(를) 풀에서 가져와 활성화했습니다.");
            objectInPool.transform.SetParent(null);
            return objectInPool;
        }
        else
        {
            GameObject objectInPool = instance.CreateObject(projectileType);
            objectInPool.SetActive(true);
            Debug.Log($"{projectileType}을(를) 풀에서 가져오지 못해 새로 생성하고 활성화했습니다.");
            objectInPool.transform.SetParent(null);
            return objectInPool;
        }
    }

    public static void ReturnObjectToQueue(GameObject obj)
    {
        if (obj != null && obj.activeSelf)
        {
            Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }

            obj.SetActive(false);
            GFunc.Log($"프로젝타일을 풀에 반환했습니다.");
            obj.transform.SetParent(instance.transform);
            instance.ObjectPool.Enqueue(obj);
        }
    }

    GameObject GetPrefabType(ProjectileType projectileType)
    {
        GameObject prefab = null;

        switch (projectileType)
        {
            case ProjectileType.CHAINBULLET:
                prefab = instance.objectPrefeb;
                break;

            case ProjectileType.BOUNCEBALL:
                prefab = instance.bouncePrefab;
                break;

            case ProjectileType.BOUNCEBULLET:
                prefab = instance.bounceSmallPrefab;
                break;

            case ProjectileType.BIGBRICK:
                prefab = instance.brickPrefab;
                break;

            default:
                Debug.LogWarning($"알 수 없는 프로젝타일 타입입니다: {projectileType}");
                break;
        }

        if (prefab == null)
        {
            Debug.LogError($"프리팹이 null입니다. ProjectileType: {projectileType}");
        }

        return prefab;
    }
}
