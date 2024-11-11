using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSuspectChoiceButton : MonoBehaviour
{
    public void Clicked()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().OpenSuspectChoice();
    }
}
