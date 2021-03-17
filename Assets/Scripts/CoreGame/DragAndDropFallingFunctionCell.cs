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
        public TriggerType triggerType;                                     // Type of drag and drop trigger
        // public DragAndDropTMPCell sourceCell;                                  // From this cell item was dragged
        public DragAndDropFallingFunctionCell destinationCell;                             // Into this cell item was dropped
        public DragAndDropFallingFunction item;                                        // Dropped item
        public bool permission;                                             // Decision need to be made on request
    }

    [Tooltip("Functional type of this cell")]
    public CellType cellType = CellType.DropOnly;                               // Special type of this cell
    [Tooltip("Sprite color for empty cell")]
    [SerializeField] public Color empty = new Color();                                       // Sprite color for empty cell
    [Tooltip("Sprite color for filled cell")]
    [SerializeField] public Color full = new Color();                                        // Sprite color for filled cell
    [Tooltip("This cell has unlimited amount of items")]
    public bool unlimitedSource = false;                                    // Item from this cell will be cloned on drag start

    private DragAndDropFallingFunction myDadItem;										// Item of this DaD cell
    private int DropCount = 0;

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
        if (myDadItem != null)
        {
            myDadItem.MakeRaycast(false);                                   // Disable item's raycast for correct drop handling
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
        if (myDadItem != null)
        {
            myDadItem.MakeRaycast(true);

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
        if (DragAndDropFallingFunction.draggingActive == true)
        {
            DragAndDropFallingFunction item = DragAndDropFallingFunction.draggedItem;
            if (item != null)
            {
                DropEventDescriptor desc = new DropEventDescriptor();
                item.HaltMovement();
                switch (cellType)                                       // Check this cell's type
                {
                    case CellType.DropOnly:                             // Item only can be dropped into destination cell
                        // Fill event descriptor
                        desc.item = item;
                        desc.destinationCell = this;
                        SendRequest(desc);                              // Send drop request
                        StartCoroutine(NotifyOnDragEnd(desc));          // Send notification after drop will be finished
                        if (desc.permission == true)                    // If drop permitted by application
                        {
                            PlaceItem(item);                            // Place dropped item in this cell
                        }
                        myDadItem.StopMovement();
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
            DropCount++;
            Debug.Log("Time to modify equations. " + DropCount.ToString());
            if (myDadItem == null)
            {
                DragAndDropCell cell = tmpObj.GetComponentInParent<DragAndDropCell>();
                tmpObj.transform.SetParent(transform, false);
                tmpObj.transform.localPosition = Vector3.zero;
                tmpObj.MakeRaycast(true);
                myDadItem = tmpObj;
            }
            else
            {
                myDadItem.ProcessCombine(tmpObj.GetComponent<FallingFunction>());
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
        if (myDadItem != null)
        {
            DropEventDescriptor desc = new DropEventDescriptor();
            // Fill event descriptor
            desc.triggerType = TriggerType.ItemWillBeDestroyed;
            desc.item = myDadItem;
            desc.destinationCell = this;
            SendNotification(desc);                                         // Notify application about item destruction
            if (myDadItem != null)
            {
                Destroy(myDadItem.gameObject);
            }
        }
        myDadItem = null;
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
            desc.triggerType = TriggerType.DropRequest;
            desc.permission = true;
            SendNotification(desc);
            result = desc.permission;
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
        desc.triggerType = TriggerType.DropEventEnd;
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
            bg.color = myDadItem != null ? full : empty;
        }
    }

    /// <summary>
    /// Updates my item
    /// </summary>
    public void UpdateMyItem()
    {
        myDadItem = GetComponentInChildren<DragAndDropFallingFunction>();
    }

    /// <summary>
    /// Get item from this cell
    /// </summary>
    /// <returns> Item </returns>
    public DragAndDropFallingFunction GetItem()
    {
        return myDadItem;
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
            desc.triggerType = TriggerType.ItemAdded;
            desc.item = newItem;
            desc.destinationCell = this;
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