using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalStorageButton : MonoBehaviour
{
    private GameManager gameManager;
    public ExternalStorageDevice storageDevice;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OpenInspection()
    {
        gameManager.OpenAssetInspection(storageDevice);
    }
}
