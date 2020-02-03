using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InputStream : MonoBehaviour
{
    public GameObject layoutGroup;
    public GameObject itemPrefab;
    public Sprite testSprite;
    [Header("Arrow Sprites")]
    public Sprite upArrow;
    public Sprite leftArrow;
    public Sprite rightArrow;
    public Sprite downArrow;
    public Sprite upLeftArrow;
    public Sprite downLeftArrow;
    public Sprite upRightArrow;
    public Sprite downRightArrow;
    public Sprite neutralArrow;
    [Header("Button Sprites")]
    public Sprite aSprite;
    public Sprite bSprite;
    public Sprite xSprite;
    public Sprite ySprite;
    public Sprite l1Sprite;
    public Sprite l2Sprite;
    public Sprite r1Sprite;
    public Sprite r2Sprite;
    Sprite currentSprite;

    List<GameObject> itemList;

    // Start is called before the first frame update
    void Start()
    {
        itemList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            AddImage();
        }
        CheckButtonInputs();
    }

    public void RecieveInput(PlayerInput.ControlStickState state)
    {
        switch (state)
        {
            case PlayerInput.ControlStickState.Neutral:
                currentSprite = neutralArrow;
                break;
            case PlayerInput.ControlStickState.Up:
                currentSprite = upArrow;
                break;
            case PlayerInput.ControlStickState.Down:
                currentSprite = downArrow;
                break;
            case PlayerInput.ControlStickState.Left:
                currentSprite = leftArrow;
                break;
            case PlayerInput.ControlStickState.Right:
                currentSprite = rightArrow;
                break;
            case PlayerInput.ControlStickState.UpLeft:
                currentSprite = upLeftArrow;
                break;
            case PlayerInput.ControlStickState.UpRight:
                currentSprite = upRightArrow;
                break;
            case PlayerInput.ControlStickState.DownLeft:
                currentSprite = downLeftArrow;
                break;
            case PlayerInput.ControlStickState.DownRight:
                currentSprite = downRightArrow;
                break;
            default:
                break;
        }
        AddImage();
    }

    public void CheckButtonInputs()
    {
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            currentSprite = aSprite;
            AddImage();
        }

        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            currentSprite = bSprite;
            AddImage();
        }

        if (Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            currentSprite = xSprite;
            AddImage();
        }

        if (Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            currentSprite = ySprite;
            AddImage();
        }

        if (Gamepad.current.leftShoulder.wasPressedThisFrame)
        {
            currentSprite = l1Sprite;
            AddImage();
        }

        if (Gamepad.current.leftTrigger.wasPressedThisFrame)
        {
            currentSprite = l2Sprite;
            AddImage();
        }

        if (Gamepad.current.rightShoulder.wasPressedThisFrame)
        {
            currentSprite = r1Sprite;
            AddImage();
        }

        if (Gamepad.current.rightTrigger.wasPressedThisFrame)
        {
            currentSprite = r2Sprite;
            AddImage();
        }

    }

    void AddImage()
    {
        GameObject newItem = Instantiate(itemPrefab);
        newItem.GetComponent<InputStreamItem>().Create(currentSprite);
        newItem.transform.SetParent(layoutGroup.transform, false);
        itemList.Add(newItem);
        if (itemList.Count > 16)
        {
            Destroy(itemList[0]);
            itemList.RemoveAt(0);
        }
    }
}
