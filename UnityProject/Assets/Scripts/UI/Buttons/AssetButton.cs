using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetButton : MonoBehaviour
{

    public GameManager gameManager;
    public Asset asset;

    public void InspectAsset()
    {
        gameManager.OpenAssetInspection(asset);
    }
}
