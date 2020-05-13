using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCutEffect : MonoBehaviour
{
    public float lingerTime;
    float lingerCounter;
    // Start is called before the first frame update
    void Start()
    {
        CreateRandomTransform();
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

    void CreateRandomTransform()
    {
        float scaleModifier = Random.Range(0.0f, 1.0f);
        float xScale = 0.5f + (0.5f * scaleModifier);
        float yscale = 2.0f + (1.0f * scaleModifier);
        transform.localScale = new Vector3(xScale, yscale, 1);

        float rotation = Random.Range(0.0f, 360.0f);
        transform.Rotate(0, 0, rotation);
    }
}
