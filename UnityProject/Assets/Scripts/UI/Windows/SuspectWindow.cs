using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuspectWindow : MonoBehaviour
{
    public AttackerSuspect suspect1;
    public AttackerSuspect suspect2;
    public AttackerSuspect suspect3;

    public TabController tabController;

    public TextMeshProUGUI discoveredAddressList;

    private void Start()
    {
        foreach (string address in GameObject.Find("GameManager").GetComponent<GameManager>().gameState.discoveredHostileIPs)
        {
            discoveredAddressList.text += "\n" + address;
        }
    }
}
