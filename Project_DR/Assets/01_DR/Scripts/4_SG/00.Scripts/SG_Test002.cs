using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_Test002 : MonoBehaviour
{
    SG_Test test;


    private void Start()
    {
        StartCoroutine(Desthis());
    }

    public void testInIt(SG_Test _test)
    {
        test = _test;
        Addthis();
    }


    public void Addthis()
    {
        test.mon.Add(this.gameObject);
    }

    private void OnDestroy()
    {
        test.mon.Remove(this.gameObject);
        test.CheckCount();
    }

    IEnumerator Desthis()
    {
        test.desSeconds++;
        yield return new WaitForSeconds(test.desSeconds);
        Destroy(this.gameObject);
    }

}
