using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public List<Tab> tabs = new List<Tab>();
    public Tab currentTab;
    public Color32 normalColour;
    public Color32 selectedColour;
    public AudioClip tabChangeSound;

    public void TabSelected(Tab selected)
    {
        //play tab change sound
        AudioSource soundSource = gameObject.GetComponent<AudioSource>();
        soundSource.loop = false;
        soundSource.clip = tabChangeSound;
        soundSource.time = 0f;
        soundSource.Play();
        //hide currently open window, and open the new window
        currentTab.CloseWindow();
        currentTab.text.color = normalColour;
        currentTab = selected;
        currentTab.text.color = selectedColour;
        currentTab.OpenWindow();
    }

    private void Start()
    {
        //hide all windows, and select the first tab
        foreach (Tab tab in tabs)
        {
            tab.CloseWindow();
            tab.text.color = normalColour;
        }

        currentTab = tabs[0];
        currentTab.text.color = selectedColour;
        currentTab.OpenWindow();
    }
}
