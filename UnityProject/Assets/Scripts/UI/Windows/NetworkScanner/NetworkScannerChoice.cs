using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkScannerChoice : MonoBehaviour
{
    private GameManager gameManager;
    public Target target;
    public Transform desktopTransform;

    [SerializeField] private GameObject assetListContent;
    [SerializeField] private GameObject assetButtonPrefab;
    [SerializeField] private GameObject assetScannerAssetWindowPrefab;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    //set up the contents of the window
    public void Initialise()
    {
        target = GameObject.FindGameObjectWithTag("Target").GetComponent<Target>();
        //build list of assets
        GameObject assetButton;
        foreach (Asset asset in target.assets)
        {
            assetButton = Instantiate(assetButtonPrefab, assetListContent.transform);
            assetButton.name = asset.assetName + " Button";
            assetButton.GetComponentInChildren<TextMeshProUGUI>().text = asset.assetName;
            assetButton.GetComponent<AssetScannerButton>().scanner = this;
            assetButton.GetComponent<AssetScannerButton>().asset = asset;
        }
    }

    public void OpenInspection(Asset asset)
    {
        Debug.Log("Opening scanner for asset: " + asset.assetName);
        //open a new window with the specified asset
        GameObject newScannerWindow = Instantiate(assetScannerAssetWindowPrefab, desktopTransform);
        newScannerWindow.name = "NetworkScanner" + asset.assetName;
        newScannerWindow.GetComponent<NetworkScannerAsset>().asset = asset;
        newScannerWindow.GetComponent<NetworkScannerAsset>().Initialise();
    }
}
