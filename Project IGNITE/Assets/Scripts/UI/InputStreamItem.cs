using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputStreamItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create(Sprite spr)
    {
        GetComponent<Image>().sprite = spr;
        GetComponent<Image>().preserveAspect = true;
    }
}
