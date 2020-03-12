using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyParticle : MonoBehaviour
{
    GameObject player;
    Vector3 startPos;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("PlayerHurtbox");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.position = Vector3.Lerp(startPos, player.transform.position, timer);
    }
}
