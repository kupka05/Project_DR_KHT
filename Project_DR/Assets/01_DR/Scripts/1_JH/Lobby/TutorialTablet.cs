using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTablet : MonoBehaviour
{
    private Image targetImage;
    public int page = 0;
    public Sprite[] images;


    // Start is called before the first frame update
    void Start()
    {
        targetImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        page = 0;
        targetImage.sprite = images[page];
    }


    public void NextSlide()
    {
        page++;
        if(page >= images.Length)
        {
            page = 0;
        }
        targetImage.sprite = images[page];
    }
}
