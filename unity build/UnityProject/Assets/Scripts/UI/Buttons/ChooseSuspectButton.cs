using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSuspectButton : MonoBehaviour
{
    public AttackerSuspect suspect;

    public void Clicked()
    {
        Debug.Log("Player suspect choice: " + suspect.name);
        GameObject.Find("GameManager").GetComponent<GameManager>().CalculateFinalScore(suspect);
    }
}
