using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Jihwan
{
    public class ItemSpawner : MonoBehaviour
    {
        public int itemID;

        // Start is called before the first frame update
        void Start()
        {
            Unit.AddFieldItem(this.transform.position, itemID);
        }
        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Unit.AddFieldItem(this.transform.position, 5401);

            }
        }

    }
}
