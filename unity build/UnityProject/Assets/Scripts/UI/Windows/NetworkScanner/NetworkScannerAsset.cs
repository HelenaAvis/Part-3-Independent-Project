using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkScannerAsset : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject windowTitle;
    [SerializeField] private GameObject trafficListContent;
    [SerializeField] private GameObject trafficEntryPrefab;

    public Asset asset;
    private Traffic.TrafficType listType;
    private List<GameObject> listEntries = new List<GameObject>();

    private void DestroyListEntries()
    {
        foreach (GameObject entry in listEntries)
        {
            Destroy(entry);
        }
    }

    private void BuildIncomingList()
    {
        DestroyListEntries();
        listType = Traffic.TrafficType.Incoming;
        Debug.Log("Building list of incoming traffic.");
           
        foreach (Traffic traffic in asset.assetTraffic)
        {
            if (traffic.type == Traffic.TrafficType.Incoming)
            {
                //create new list entry
                GameObject listEntry = Instantiate(trafficEntryPrefab, trafficListContent.transform);
                listEntry.GetComponent<TrafficEntry>().traffic = traffic;
                //set the text of the list entry
                listEntry.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = traffic.port.portNumber.ToString();
                listEntry.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = traffic.source.address;
                listEntry.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = traffic.destination;
                listEntries.Add(listEntry);
            }
        }
    }

    private void BuildOutgoingList()
    {
        DestroyListEntries();
        listType = Traffic.TrafficType.Outgoing;
        Debug.Log("Building list of outgoing traffic.");
        
        foreach (Traffic traffic in asset.assetTraffic)
        {
            if (traffic.type == Traffic.TrafficType.Outgoing)
            {
                //create new list entry
                GameObject listEntry = Instantiate(trafficEntryPrefab, trafficListContent.transform);
                listEntry.GetComponent<TrafficEntry>().traffic = traffic;
                //set the text of the list entry
                listEntry.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = traffic.port.portNumber.ToString();
                listEntry.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = traffic.source.sourceName;
                listEntry.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = traffic.destination;
                listEntries.Add(listEntry);
            }
        }
    }

    public void Initialise()
    {
        //set asset name
        windowTitle.GetComponent<TextMeshProUGUI>().text = "Asset Scanner - " + asset.assetName;
        title.GetComponent<TextMeshProUGUI>().text = asset.assetName;

        //build list of incoming traffic
        BuildIncomingList();
    }

    public void SwitchTrafficLists()
    {
        if (listType == Traffic.TrafficType.Incoming)
        {
            //build outgoing list
            BuildOutgoingList();
        } else
        {
            //build the list of incoming traffic
            BuildIncomingList();
        }
    }
}
