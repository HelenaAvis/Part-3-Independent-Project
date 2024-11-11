using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetScannerButton : MonoBehaviour
{
    public NetworkScannerChoice scanner;
    public Asset asset;

    public void OpenScanner()
    {
        scanner.OpenInspection(asset);
    }
}
