using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneWall : MonoBehaviour
{
    public LineRenderer line;
    public GameObject droneA;
    public GameObject droneB;

    public float lingerTime;
    float lingerCounter;
    // Start is called before the first frame update
    void Start()
    {
        line.SetPosition(0, droneA.transform.localPosition);
        line.SetPosition(1, droneB.transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        lingerCounter += Time.deltaTime;
        if (lingerCounter >= lingerTime)
        {
            Destroy(gameObject);
        }
    }
}
