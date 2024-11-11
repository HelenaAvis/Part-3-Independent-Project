using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidebookButton : MonoBehaviour
{
    [SerializeField] private GameObject guidebookPrefab;
    [SerializeField] private GameObject background;

    public void Clicked()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().OpenGuidebook();
    }

    public void OpenStartMenuGuidebook()
    {
        GameObject guidebookWindow = Instantiate(guidebookPrefab, background.transform);
        guidebookWindow.name = "Guidebook";
    }
}
