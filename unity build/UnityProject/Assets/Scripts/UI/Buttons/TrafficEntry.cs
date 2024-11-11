using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrafficEntry : MonoBehaviour
{
    [SerializeField] private GameObject portText;
    [SerializeField] private GameObject sourceText;
    [SerializeField] private GameObject destinationText;
    [SerializeField] private Color32 hoverColour;
    [SerializeField] private Color32 normalColour;

    public Traffic traffic;

    private void OnMouseEnter()
    {
        //change colour of text to white
        portText.GetComponent<TextMeshProUGUI>().color = hoverColour;
        sourceText.GetComponent<TextMeshProUGUI>().color = hoverColour;
        destinationText.GetComponent<TextMeshProUGUI>().color = hoverColour;
    }

    private void OnMouseExit()
    {
        //change colour of text back to green
        portText.GetComponent<TextMeshProUGUI>().color = normalColour;
        sourceText.GetComponent<TextMeshProUGUI>().color = normalColour;
        destinationText.GetComponent<TextMeshProUGUI>().color = normalColour;
    }

    public void Clicked()
    {
        //change colour of text back to green
        portText.GetComponent<TextMeshProUGUI>().color = normalColour;
        sourceText.GetComponent<TextMeshProUGUI>().color = normalColour;
        destinationText.GetComponent<TextMeshProUGUI>().color = normalColour;

        //register evidence if needed
        if (traffic.hostile)
        {
            if (traffic.type == Traffic.TrafficType.Outgoing && !GameObject.Find("Target").GetComponent<Target>().assetIPs.Contains(traffic.destination))
            {
                //if traffic is outgoing, and the destination is not another target asset, add the destination to the list of known hostile addresses
                if (!GameObject.Find("GameState").GetComponent<State>().discoveredHostileIPs.Contains(traffic.destination))
                {
                    GameObject.Find("GameState").GetComponent<State>().discoveredHostileIPs.Add(traffic.destination);
                }
            }
            GameObject.Find("GameManager").GetComponent<GameManager>().DiscoverEvidence(traffic);
        }
    }
}
