using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableWindow : MonoBehaviour, IDragHandler
{
    [SerializeField] private RectTransform windowTransform;

    private void Start()
    {
        gameObject.AddComponent(typeof(BringToFront));
    }

    public void OnDrag(PointerEventData eventData)
    {
        windowTransform.anchoredPosition += eventData.delta;
    }

    public void Close()
    {
        Destroy(windowTransform.gameObject);
    }
}
