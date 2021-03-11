using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NumberSlot : MonoBehaviour, IDropHandler
{

    private float initialXSize;
    private float initialYSize;
    private void Start()
    {
        initialXSize = transform.localScale.x;
        initialYSize = transform.localScale.y;
    }
    public void OnDrop(PointerEventData pointerEventData)
    {
        Debug.Log("Drop Event Complete");
        if (pointerEventData.pointerDrag != null)
        {
            pointerEventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
