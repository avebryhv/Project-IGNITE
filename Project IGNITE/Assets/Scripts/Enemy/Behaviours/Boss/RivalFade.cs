using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalFade : MonoBehaviour
{
    public float killAfterTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        killAfterTime -= Time.deltaTime;
        if (killAfterTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
