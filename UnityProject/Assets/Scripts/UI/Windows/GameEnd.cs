using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeTaken;
    [SerializeField] private Image timeStar1;
    [SerializeField] private Image timeStar2;
    [SerializeField] private Image timeStar3;
    [SerializeField] private Image timeStar4;
    [SerializeField] private Image timeStar5;

    [SerializeField] private TextMeshProUGUI evidenceFound;
    [SerializeField] private Image evidenceStar1;
    [SerializeField] private Image evidenceStar2;
    [SerializeField] private Image evidenceStar3;
    [SerializeField] private Image evidenceStar4;
    [SerializeField] private Image evidenceStar5;

    [SerializeField] private TextMeshProUGUI suspectChoice;
    [SerializeField] private TextMeshProUGUI totalScore;

    private State gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<State>();

        GameObject.Find("GameState").transform.SetParent(gameObject.transform);
        GameObject.Find("Target").transform.SetParent(gameObject.transform);
        GameObject.Find("Suspect1").transform.SetParent(gameObject.transform);
        GameObject.Find("Suspect2").transform.SetParent(gameObject.transform);
        GameObject.Find("Suspect3").transform.SetParent(gameObject.transform);

        //setup time score
        timeTaken.text = "Time Taken: " + String.Format("Time - {0}:{1}{2}", Mathf.FloorToInt(gameState.timeTaken / 60), Mathf.FloorToInt((gameState.timeTaken % 60) / 10), Mathf.FloorToInt((gameState.timeTaken % 60) % 10));
        switch (gameState.timeStars)
        {
            case 1:
                timeStar1.enabled = true;
                timeStar2.enabled = false;
                timeStar3.enabled = false;
                timeStar4.enabled = false;
                timeStar5.enabled = false;
                break;
            case 2:
                timeStar1.enabled = true;
                timeStar2.enabled = true;
                timeStar3.enabled = false;
                timeStar4.enabled = false;
                timeStar5.enabled = false;
                break;
            case 3:
                timeStar1.enabled = true;
                timeStar2.enabled = true;
                timeStar3.enabled = true;
                timeStar4.enabled = false;
                timeStar5.enabled = false;
                break;
            case 4:
                timeStar1.enabled = true;
                timeStar2.enabled = true;
                timeStar3.enabled = true;
                timeStar4.enabled = true;
                timeStar5.enabled = false;
                break;
            case 5:
                timeStar1.enabled = true;
                timeStar2.enabled = true;
                timeStar3.enabled = true;
                timeStar4.enabled = true;
                timeStar5.enabled = true;
                break;
        }

        //setup evidence score
        evidenceFound.text = "Evidence Found: " + gameState.discoveredEvidence.Count + "/" + gameState.evidenceList.Count;
        switch (gameState.evidenceStars)
        {
            case 1:
                evidenceStar1.enabled = true;
                evidenceStar2.enabled = false;
                evidenceStar3.enabled = false;
                evidenceStar4.enabled = false;
                evidenceStar5.enabled = false;
                break;
            case 2:
                evidenceStar1.enabled = true;
                evidenceStar2.enabled = true;
                evidenceStar3.enabled = false;
                evidenceStar4.enabled = false;
                evidenceStar5.enabled = false;
                break;
            case 3:
                evidenceStar1.enabled = true;
                evidenceStar2.enabled = true;
                evidenceStar3.enabled = true;
                evidenceStar4.enabled = false;
                evidenceStar5.enabled = false;
                break;
            case 4:
                evidenceStar1.enabled = true;
                evidenceStar2.enabled = true;
                evidenceStar3.enabled = true;
                evidenceStar4.enabled = true;
                evidenceStar5.enabled = false;
                break;
            case 5:
                evidenceStar1.enabled = true;
                evidenceStar2.enabled = true;
                evidenceStar3.enabled = true;
                evidenceStar4.enabled = true;
                evidenceStar5.enabled = true;
                break;
        }

        //setup suspect choice
        if (gameState.correctSuspectChoice)
        {
            suspectChoice.text = "Correct Suspect Choice: " + gameState.suspectScore;
        } else
        {
            suspectChoice.text = "Incorrect Suspect Choice: " + gameState.suspectScore;
        }

        //setup total score
        totalScore.text = "Total Score: " + gameState.score;
    }
}
