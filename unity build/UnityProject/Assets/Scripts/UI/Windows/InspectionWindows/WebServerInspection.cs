using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WebServerInspection : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject titleText;
    [SerializeField] private GameObject topbarText;
    [SerializeField] private GameObject fileListContent;
    [SerializeField] private GameObject fileListButtonPrefab;
    [SerializeField] private GameObject storageDeviceButtonArea;
    [SerializeField] private GameObject storageDeviceButtonPrefab;
    [SerializeField] private GameObject nonePrefab;
    [SerializeField] private GameObject statusText;

    public Asset asset;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    //set up the contents of the window
    public void Initialise()
    {
        //set asset name
        topbarText.GetComponent<TextMeshProUGUI>().text = "Asset Inspection - " + asset.assetName;
        titleText.GetComponent<TextMeshProUGUI>().text = asset.assetName;

        GameObject fileButton;
        //build the list of files
        foreach (File file in asset.files)
        {
            //create a new instance of the button prefab for the file
            fileButton = Instantiate(fileListButtonPrefab, fileListContent.transform);
            fileButton.name = file.filename + " Button";
            fileButton.GetComponentInChildren<TextMeshProUGUI>().text = file.filename;
            fileButton.GetComponent<FileButton>().file = file;
            fileButton.GetComponent<FileButton>().gameManager = gameManager;
            if (file.hostile)
            {
                fileButton.GetComponent<Button>().onClick.AddListener(delegate { gameManager.DiscoverEvidence(file); });
            }
        }

        //if there is a connected device, add a button for it
        if (asset.externalStorageDeviceConnected)
        {
            GameObject storageButton = Instantiate(storageDeviceButtonPrefab, storageDeviceButtonArea.transform);
            storageButton.name = asset.connectedDevice.assetName + " Button";
            storageButton.GetComponent<ExternalStorageButton>().storageDevice = asset.connectedDevice;
        }
        else
        {
            GameObject noneText = Instantiate(nonePrefab, storageDeviceButtonArea.transform);
        }

        //get the status of the website
        WebServer webServer = (WebServer)asset;
        if (webServer.webSiteAttacked)
        {
            statusText.GetComponent<TextMeshProUGUI>().text = "Website Status: Compromised";
        } else
        {
            statusText.GetComponent<TextMeshProUGUI>().text = "Website Status: Normal";
        }
    }
}
