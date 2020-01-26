using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    PlayerScriptFinder finder;
    public GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CStickInput(Vector2 input)
    {
        if (input.magnitude == 0)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
            arrow.transform.right = input;
        }
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
