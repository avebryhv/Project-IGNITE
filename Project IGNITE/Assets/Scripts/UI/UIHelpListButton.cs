using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIHelpListButton : MonoBehaviour, ISelectHandler
{
    public TextMeshProUGUI text;
    public Selectable button;
    public int index;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateButton(string name, int i)
    {
        text.text = name;
        index = i;
    }

    public void OnSelect(BaseEventData eventData)
    {

        FindObjectOfType<UIHelpList>().SetIndex(index);
    }
}
