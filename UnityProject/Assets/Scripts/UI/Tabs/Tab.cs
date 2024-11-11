using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class Tab : MonoBehaviour
{
    public GameObject display;
    public TabController controller;
    public TextMeshProUGUI text;

    private void Start()
    {
        controller.tabs.Add(this);
    }

    public void Selected()
    {
        controller.TabSelected(this);
    }

    public void OpenWindow()
    {
        display.SetActive(true);
    }

    public void CloseWindow()
    {
        display.SetActive(false);
    }
}
