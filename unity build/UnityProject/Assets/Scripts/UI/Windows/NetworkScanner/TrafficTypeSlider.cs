using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrafficTypeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private NetworkScannerAsset scanner;


    private void Start()
    {
        slider.onValueChanged.AddListener(delegate { scanner.SwitchTrafficLists(); });
    }
}
