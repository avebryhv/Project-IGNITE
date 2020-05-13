using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHelpList : MonoBehaviour
{
    public GameObject layoutGroup;
    public List<UIHelpListItem> items;
    public GameObject itemPrefab;
    UIHelpListItem currentItem;
    GameObject lastItem;
    List<Button> buttonList;
    int selectedIndex;
    int lastSelectedIndex;
    public Image displayImage;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    private void Start()
    {
        buttonList = new List<Button>();
        currentItem = items[0];
        CreateButtons();
        FindObjectOfType<PauseMenu>().helpFirstSelection = buttonList[0];
        buttonList[0].Select();

        layoutGroup.transform.Translate(new Vector3(0, -99999, 0));
        //Debug.Log(layoutGroup.GetComponent<RectTransform>().rect.width);

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
        newItem.GetComponentInChildren<UIHelpListButton>().CreateButton(currentItem.name, index);
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
            yPos = 0 + (50 * (index - midPoint));
        }
        else
        {
            yPos = 0 - (50 * (midPoint - index));
        }

        //Debug.Log("current index " + index);
        //layoutGroup.transform.position = new Vector2(layoutGroup.transform.position.x, yPos);
        layoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(layoutGroup.GetComponent<RectTransform>().anchoredPosition.x, yPos);
        title.text = items[index].name;
        description.text = items[index].description;
    }
}
