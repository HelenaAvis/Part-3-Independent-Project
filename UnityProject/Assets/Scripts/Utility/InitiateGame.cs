using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InitiateGame : MonoBehaviour
{
    [SerializeField] private GameObject gameStateObject;
    private State gameState;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject targetObject;
    private Target target;
    [SerializeField] private GameObject suspect1Object;
    private AttackerSuspect suspect1;
    [SerializeField] private GameObject suspect2Object;
    private AttackerSuspect suspect2;
    [SerializeField] private GameObject suspect3Object;
    private AttackerSuspect suspect3;
    [SerializeField] private int randomAttackChance;
    [SerializeField] private GameObject startButton;

    private static System.Random rng = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        startButton.SetActive(false);

        gameStateObject = GameObject.Find("GameState");
        gameState = gameStateObject.GetComponent<State>();

        //make the target, attacker, and suspects persist between scenes
        DontDestroyOnLoad(gameStateObject);
        DontDestroyOnLoad(targetObject);
        DontDestroyOnLoad(suspect1Object);
        DontDestroyOnLoad(suspect2Object);
        DontDestroyOnLoad(suspect3Object);

        target = targetObject.GetComponent<Target>();
        suspect1 = suspect1Object.GetComponent<AttackerSuspect>();
        suspect2 = suspect2Object.GetComponent<AttackerSuspect>();
        suspect3 = suspect3Object.GetComponent<AttackerSuspect>();
        
        statusText.SetText("Generating Target...");

        //TARGET ASSET GENERATION
        statusText.SetText("Generating Target Assets...");
        target.GenerateAssets();

        //TARGET FILE/EMAIL/TRAFFIC GENERATION
        target.GenerateFiles();
        target.GenerateMail();
        target.GenerateTraffic();

        //ATTACKER GENERATION
        statusText.SetText("Generating Suspects...");
        //generate guilty suspect and 2 other suspects
        int guiltySuspect = rng.Next(1,4);
        switch(guiltySuspect)
        {
            case 1:
                suspect1.GenerateAttacker(target, true, randomAttackChance);
                suspect1.GenerateEvidence(target);
                suspect2.GenerateAttacker(target, false, randomAttackChance);
                suspect3.GenerateAttacker(target, false, randomAttackChance);
                break;
            case 2:
                suspect1.GenerateAttacker(target, false, randomAttackChance);
                suspect2.GenerateAttacker(target, true, randomAttackChance);
                suspect2.GenerateEvidence(target);
                suspect3.GenerateAttacker(target, false, randomAttackChance);
                break;
            case 3:
                suspect1.GenerateAttacker(target, false, randomAttackChance);
                suspect2.GenerateAttacker(target, false, randomAttackChance);
                suspect3.GenerateAttacker(target, true, randomAttackChance);
                suspect3.GenerateEvidence(target);
                break;
        };

        statusText.SetText("Finished generation!");
        statusText.gameObject.SetActive(false);
        startButton.SetActive(true);
    }
}
