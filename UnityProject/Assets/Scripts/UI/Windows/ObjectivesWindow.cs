using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectivesWindow : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject accessCompletion;
    [SerializeField] private GameObject accessDescription;
    [SerializeField] private GameObject evidenceCompletion;
    [SerializeField] private GameObject evidenceDescription;
    [SerializeField] private GameObject suspectDescription;

    private string targetName;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        targetName = GameObject.Find("Target").GetComponent<Target>().targetName;
        accessDescription.GetComponent<TextMeshProUGUI>().text = "Find evidence of how the attacker gained access to " + targetName + "'s assets.";
        evidenceDescription.GetComponent<TextMeshProUGUI>().text = "Analyse assets and network traffic to find evidence of the attack against " + targetName + ".";
        suspectDescription.GetComponent<TextMeshProUGUI>().text = "Choose the suspect responsible for the attack against " + targetName + ".";

        Debug.Log("Discovered pieces of access evidence: " + gameManager.gameState.CountDiscoveredEvidence(true) + "/" + gameManager.gameState.CountAccessEvidence());
        Debug.Log("Discovered pieces of evidence: " + gameManager.gameState.CountDiscoveredEvidence(false) + "/" + gameManager.gameState.evidenceList.Count);
    }

    // Update is called once per frame
    void Update()
    {
        //update the completion percentages
        if (gameObject != null)
        {
            accessCompletion.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(((float)gameManager.gameState.CountDiscoveredEvidence(true) / (float)gameManager.gameState.CountAccessEvidence()) * 100f) + "%";
            evidenceCompletion.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(((float)gameManager.gameState.CountDiscoveredEvidence(false) / (float)gameManager.gameState.evidenceList.Count) * 100f) + "%";
        }
    }
}
