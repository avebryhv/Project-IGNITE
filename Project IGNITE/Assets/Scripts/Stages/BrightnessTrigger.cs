using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BrightnessTrigger : MonoBehaviour
{
    public Light2D globalLight;
    public float intensity;
    bool lerpingIntensity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHurtbox")
        {
            SetIntensity(intensity);
        }


    }

    public void SetIntensity(float inten)
    {
        globalLight.intensity = inten;
    }
}
