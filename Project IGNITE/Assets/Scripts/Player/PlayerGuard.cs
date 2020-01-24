using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuard : MonoBehaviour
{
    PlayerScriptFinder finder;

    public float timeHeld;
    public float parryTiming;
    public float guardDuration;
    public bool isGuarding;

    // Start is called before the first frame update
    void Start()
    {
        timeHeld = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGuarding)
        {
            OnGuardHold();
        }
    }

    public void InputGuardHeld()
    {
        
    }

    public void OnGuardPress()
    {
        if (!finder.movement.inDash)
        {
            isGuarding = true;
            timeHeld = 0;
        }
        
    }

    public void OnGuardHold()
    {
        if (timeHeld <= parryTiming)
        {
            finder.sprite.ChangeSpriteColour(Color.blue);
        }
        else
        {
            finder.sprite.ChangeSpriteColour(Color.white);
        }

        if (timeHeld >= guardDuration)
        {
            OnGuardRelease();
        }
        timeHeld += Time.deltaTime;
    }

    public void OnGuardRelease()
    {
        timeHeld = 0;
        isGuarding = false;
    }

    public void SetFinder(PlayerScriptFinder f)
    {
        finder = f;
    }
}
