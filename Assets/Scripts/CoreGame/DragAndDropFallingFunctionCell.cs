using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Every item's cell must contain this script
/// </summary>
[RequireComponent(typeof(Image))]


public class DragAndDropFallingFunctionCell : MonoBehaviour,  IDropHandler
{
    public enum CellType                                                    // Cell types
    {
        DropOnly                                                          // Item will be dropped into cell
    }

    public enum TriggerType                                                 // Types of drag and drop events
    {
        DropRequest,                                                        // Request for item drop from one cell to another
        DropEventEnd,                                                       // Drop event completed
        ItemAdded,                                                          // Item manualy added into cell
        ItemWillBeDestroyed                                                 // Called just before item will be destroyed
    }

    public class DropEventDescriptor                                        // Info about item's drop event
    {
        public TriggerType TriggerType;                                     // Type of drag and drop trigger
        // public DragAndDropTMPCell sourceCell;                                  // From this cell item was dragged
        public DragAndDropFallingFunctionCell DestinationCell;                             // Into this cell item was dropped
        public DragAndDropFallingFunction Item;                                        // Dropped item
        public bool Permission;                                             // Decision need to be made on request
    }

    [Tooltip("Functional type of this cell")]
    public CellType cellType = CellType.DropOnly;                               // Special type of this cell
    [Tooltip("Sprite color for empty cell")]
    [SerializeField] public Color empty = new Color();                                       // Sprite color for empty cell
    [Tooltip("Sprite color for filled cell")]
    [SerializeField] public Color full = new Color();                                        // Sprite color for filled cell
    [Tooltip("This cell has unlimited amount of items")]
    public bool unlimitedSource = false;                                    // Item from this cell will be cloned on drag start
    [SerializeField] public TMPro.TextMeshProUGUI linkedPointsDisplay;

    private DragAndDropFallingFunction _myDadItem;										// Item of this DaD cell
    private FallingFunction _myFunction;
    private int _dropCount = 0;

    void OnEnable()
    {
        DragAndDropFallingFunction.OnItemDragStartEvent += OnAnyItemDragStart;         // Handle any item drag start
        DragAndDropFallingFunction.OnItemDragEndEvent += OnAnyItemDragEnd;             // Handle any item drag end
        UpdateMyItem();
        UpdateBackgroundState();
    }

    void OnDisable()
    {
        DragAndDropFallingFunction.OnItemDragStartEvent -= OnAnyItemDragStart;
        DragAndDropFallingFunction.OnItemDragEndEvent -= OnAnyItemDragEnd;
        StopAllCoroutines();                                                // Stop all coroutines if there is any
    }

    /// <summary>
    /// On any item drag start need to disable all items raycast for correct drop operation
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragStart(DragAndDropFallingFunction item)
    {
        UpdateMyItem();
        if (_myDadItem != null)
        {
            _myDadItem.MakeRaycast(false);                                   // Disable item's raycast for correct drop handling
        }
    }

    /// <summary>
    /// On any item drag end enable all items raycast
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragEnd(DragAndDropFallingFunction item)
    {
        Debug.Log("OnAnyItemDragEnd Executed!");
        UpdateMyItem();
        if (_myDadItem != null)
        {
            _myDadItem.MakeRaycast(true);

        }
        UpdateBackgroundState();
    }

    /// <summary>
    /// Item is dropped in this cell
    /// </summary>
    /// <param name="data"></param>
    public void OnDrop(PointerEventData data)
    {
        Debug.Log("OnDrop Executed!");
        if (DragAndDropFallingFunction.DraggingActive == true)
        {
            DragAndDropFallingFunction item = DragAndDropFallingFunction.DraggedItem;
            if (item != null)
            {
                DropEventDescriptor desc = new DropEventDescriptor();
                item.HaltMovement();
                switch (cellType)                                       // Check this cell's type
                {
                    case CellType.DropOnly:                             // Item only can be dropped into destination cell
                        // Fill event descriptor
                        desc.Item = item;
                        desc.DestinationCell = this;
                        SendRequest(desc);                              // Send drop request
                        StartCoroutine(NotifyOnDragEnd(desc));          // Send notification after drop will be finished
                        if (desc.Permission == true)                    // If drop permitted by application
                        {
                            PlaceItem(item);                            // Place dropped item in this cell
                        }
                        _myDadItem.StopMovement();
                        break;
                    default:
                        break;
                }
            }
            UpdateMyItem();
            UpdateBackgroundState();
        }
    }

    /// <summary>
    /// Put item into this cell.
    /// </summary>
    /// <param name="tmpObj">A TMP object....  This code is not clean.</param>
    private void PlaceItem(DragAndDropFallingFunction tmpObj)
    {
        if (tmpObj != null)
        {
            _dropCount++;
            Debug.Log("Time to modify equations. " + _dropCount.ToString());
            if (_myDadItem == null)
            {
                DragAndDropCell cell = tmpObj.GetComponentInParent<DragAndDropCell>();
                tmpObj.transform.SetParent(transform, false);
                tmpObj.transform.localPosition = Vector3.zero;
                tmpObj.MakeRaycast(true);
                _myDadItem = tmpObj;
            }
            else
            {
                _myDadItem.ProcessCombine(tmpObj.GetComponent<FallingFunction>());
            }
        }
        UpdateBackgroundState();
    }

    /// <summary>
    /// Destroy item in this cell
    /// </summary>
    private void DestroyItem()
    {
        UpdateMyItem();
        if (_myDadItem != null)
        {
            DropEventDescriptor desc = new DropEventDescriptor();
            // Fill event descriptor
            desc.TriggerType = TriggerType.ItemWillBeDestroyed;
            desc.Item = _myDadItem;
            desc.DestinationCell = this;
            SendNotification(desc);                                         // Notify application about item destruction
            if (_myDadItem != null)
            {
                Destroy(_myDadItem.gameObject);
            }
        }
        _myDadItem = null;
        UpdateBackgroundState();
    }

    /// <summary>
    /// Send drag and drop information to application
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
    private void SendNotification(DropEventDescriptor desc)
    {
        if (desc != null)
        {
            // Send message with DragAndDrop info to parents GameObjects
            gameObject.SendMessageUpwards("OnSimpleDragAndDropEvent", desc, SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Send drag and drop request to application
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
    /// <returns> result from desc.permission </returns>
    private bool SendRequest(DropEventDescriptor desc)
    {
        bool result = false;
        if (desc != null)
        {
            desc.TriggerType = TriggerType.DropRequest;
            desc.Permission = true;
            SendNotification(desc);
            result = desc.Permission;
        }
        return result;
    }

    /// <summary>
    /// Wait for event end and send notification to application
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
    /// <returns></returns>
    private IEnumerator NotifyOnDragEnd(DropEventDescriptor desc)
    {
        // Wait end of drag operation
        while (DragAndDropItem.draggedItem != null)
        {
            yield return new WaitForEndOfFrame();
        }
        desc.TriggerType = TriggerType.DropEventEnd;
        SendNotification(desc);
    }

    /// <summary>
    /// Change cell's sprite color on item put/remove.
    /// </summary>
    /// <param name="condition"> true - filled, false - empty </param>
    public void UpdateBackgroundState()
    {
        Image bg = GetComponent<Image>();
        if (bg != null)
        {
            bg.color = _myDadItem != null ? full : empty;
        }
    }

    /// <summary>
    /// Updates my item
    /// </summary>
    public void UpdateMyItem()
    {
        _myDadItem = GetComponentInChildren<DragAndDropFallingFunction>();
        _myFunction = GetComponentInChildren<FallingFunction>();
        if (_myFunction)
        {
            linkedPointsDisplay.gameObject.SetActive(true);
            _myFunction.SetPointsDisplay(linkedPointsDisplay);
        }
    }

    /// <summary>
    /// Get item from this cell
    /// </summary>
    /// <returns> Item </returns>
    public DragAndDropFallingFunction GetItem()
    {
        return _myDadItem;
    }

    /// <summary>
    /// Manualy add item into this cell
    /// </summary>
    /// <param name="newItem"> New item </param>
    public void AddItem(DragAndDropFallingFunction newItem)
    {
        if (newItem != null)
        {
            PlaceItem(newItem);
            DropEventDescriptor desc = new DropEventDescriptor();
            // Fill event descriptor
            desc.TriggerType = TriggerType.ItemAdded;
            desc.Item = newItem;
            desc.DestinationCell = this;
            SendNotification(desc);
        }
    }

    /// <summary>
    /// Manualy delete item from this cell
    /// </summary>
    public void RemoveItem()
    {
        DestroyItem();
    }


}