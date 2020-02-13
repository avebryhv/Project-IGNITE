using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class UIMoveList : MonoBehaviour
{
    public GameObject layoutGroup;
    public List<UIMoveListItem> items;
    public GameObject itemPrefab;
    UIMoveListItem currentItem;
    GameObject lastItem;
    List<Button> buttonList;
    int selectedIndex;
    int lastSelectedIndex;
    public Image displayImage;

    private void Start()
    {
        buttonList = new List<Button>();
        currentItem = items[0];
        CreateButtons();
        FindObjectOfType<PauseMenu>().moveListFirstSelection = buttonList[0];
        buttonList[0].Select();
        
        layoutGroup.transform.Translate(new Vector3(0, -99999, 0));
        Debug.Log(layoutGroup.GetComponent<RectTransform>().rect.width);
        
    }

    private void OnEnable()
    {
        //if (buttonList[0] != null)
        //{
        //    buttonList[0].Select();
        //    SetIndex(0);
        //}
    }

    public void CreateButtons()
    {
        for (int i = 0; i < items.Count; i++)
        {
            currentItem = items[i];
            CreateNewItem(i);
        }
    }

    private void Update()
    {
        
    }

    public void CreateNewItem(int index)
    {
        GameObject newItem = Instantiate(itemPrefab);
        newItem.transform.parent = layoutGroup.transform;
        newItem.GetComponentInChildren<UIMoveListButton>().CreateButton(currentItem.name, currentItem.image, index);
        buttonList.Add(newItem.GetComponentInChildren<Button>());

        if (lastItem != null)
        {
            //Set Current Button Upwards Navigation
            var nav = newItem.GetComponentInChildren<Button>().navigation;
            nav.selectOnUp = lastItem.GetComponentInChildren<Button>();
            newItem.GetComponentInChildren<Button>().navigation = nav;

            //Set Last Button Downwards Navigation
            var nav2 = lastItem.GetComponentInChildren<Button>().navigation;
            nav2.selectOnDown = newItem.GetComponentInChildren<Button>();
            lastItem.GetComponentInChildren<Button>().navigation = nav2;
        }



        lastItem = newItem;
    }

    public void SetIndex(int index)
    {
        int midPoint = Mathf.FloorToInt(buttonList.Count / 2.0f);
        //if (buttonList.Count % 2 != 0)
        //{
        //    midPoint += 1;
        //}
        float yPos;

        if (index > midPoint)
        {
            yPos = 0 + (50 * (index - midPoint + 1));
        }
        else
        {
            yPos = 0 - (50 * (midPoint - index + 1));
        }

        Debug.Log("current index " + index);
        //layoutGroup.transform.position = new Vector2(layoutGroup.transform.position.x, yPos);
        layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(layoutGroup.GetComponent<RectTransform>().anchoredPosition.x, yPos);
        displayImage.sprite = items[index].image;
    }

    
}
