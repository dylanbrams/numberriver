using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NumberSlot : MonoBehaviour, IDropHandler
{

    private float _initialXSize;
    private float _initialYSize;
    private void Start()
    {
        _initialXSize = transform.localScale.x;
        _initialYSize = transform.localScale.y;
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
