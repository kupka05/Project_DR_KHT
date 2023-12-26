using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject objectPrefeb;
    public GameObject bouncePrefab;
    public GameObject bounceSmallPrefab;

    Queue<GameObject> ObjectPool = new Queue<GameObject>(); //오브젝트를 담을 큐
    public static ObjectPoolManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            for (int i = 0; i < 100; i++)
            {
                CreateObject(); //초기에 n개의 오브젝트를 생성함
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    GameObject CreateObject() //초기 OR 오브젝트 풀에 남은 오브젝트가 부족할 때, 오브젝트를 생성하기위해 호출되는 함수
    {
        GameObject newObj = Instantiate(objectPrefeb, instance.transform);
        newObj.gameObject.SetActive(false);

        return newObj;
    }
    public static GameObject GetObject() //오프젝트가 필요할 때 다른 스크립트에서 호출되는 함수
    {
        if (instance.ObjectPool.Count > 0) //현재 큐에 남아있는 오브젝트가 있다면,
        {
            GameObject objectInPool = instance.ObjectPool.Dequeue();

            objectInPool.gameObject.SetActive(true);
            objectInPool.transform.SetParent(null);
            return objectInPool;
        }
        else //큐에 남아있는 오브젝트가 없을 때 새로 만들어서 사용
        {
            GameObject objectInPool = instance.CreateObject();

            objectInPool.gameObject.SetActive(true);
            objectInPool.transform.SetParent(null);
            return objectInPool;
        }
    }
    public static void ReturnObjectToQueue(GameObject obj)
    {
        if (obj != null && obj.activeSelf) // obj가 null이 아니고 활성화 상태인지 확인
        {
            Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }
            else
                return;

            SphereCollider sphereCollider = obj.GetComponent<SphereCollider>();
            if(sphereCollider != null)
            {
                sphereCollider.isTrigger = true;
            }

            obj.SetActive(false);
            obj.transform.SetParent(instance.transform);
            instance.ObjectPool.Enqueue(obj);
        }
    }
}