using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{

    public enum State { Monster, Item, Quest}

    public State state ;
    private TMP_Text resultName;
    private TMP_Text resultCount;
    private TMP_Text resultGold;
    private TMP_Text resultExp;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
