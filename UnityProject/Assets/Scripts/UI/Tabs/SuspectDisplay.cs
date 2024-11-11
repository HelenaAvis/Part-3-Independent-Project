using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuspectDisplay : MonoBehaviour
{
    public int suspectNumber;
    public AttackerSuspect suspect;
    public TextMeshProUGUI type;
    public TextMeshProUGUI addressList;
    public ChooseSuspectButton chooseButton;

    private void Start()
    {
        suspect = GameObject.Find("Suspect" + suspectNumber).GetComponent<AttackerSuspect>();
        chooseButton.suspect = suspect;
        type.text = "Type: " + suspect.attackerType.ToString();
        switch (suspect.attackerType)
        {
            case AttackerSuspect.AttackerType.StateSponsored:
                type.text = "State Sponsored";
                break;
            case AttackerSuspect.AttackerType.IndependentHacker:
                type.text = "Independent Hacker";
                break;
            case AttackerSuspect.AttackerType.Criminal:
                type.text = "Criminal";
                break;
            case AttackerSuspect.AttackerType.Hacktivist:
                type.text = "Hacktivist";
                break;
            case AttackerSuspect.AttackerType.RivalCompany:
                type.text = "Rival Company";
                break;
        }

        foreach (string address in suspect.knownAddresses)
        {
            addressList.text += "\n" + address;
        }
    }
}
