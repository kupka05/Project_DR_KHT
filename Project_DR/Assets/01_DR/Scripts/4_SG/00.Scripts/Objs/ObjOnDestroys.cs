using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjOnDestroys : MonoBehaviour
{
    private void OnDestroy()
    {
        Destroy(this);
    }

}
