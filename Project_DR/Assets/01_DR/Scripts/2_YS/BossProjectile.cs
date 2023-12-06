using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public GameObject explosionPrefab;

    public bool isDelay = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Floor"))
        {
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        if(!isDelay)
        {
            isDelay = true;
            yield return new WaitForSeconds(2.0f);
            isDelay = false;
            Destroy(this.gameObject);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

    }

   
}
