using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropController : MonoBehaviour
{
    /// <summary>
    /// Operate all drag and drop requests and events from children cells
    /// </summary>
    /// <param name="desc"> request or event descriptor </param>
    void OnSimpleDragAndDropEvent(DragAndDropFallingFunctionCell.DropEventDescriptor desc)
    {
        // Get control unit of destination cell
        DragAndDropController destinationSheet = desc.destinationCell.GetComponentInParent<DragAndDropController>();
        switch (desc.triggerType)                                               // What type event is?
        {
            case DragAndDropFallingFunctionCell.TriggerType.DropRequest:                       // Request for item drag (note: do not destroy item on request)
                Debug.Log("Request " + desc.item.name + " to " + destinationSheet.name);
                break;
            case DragAndDropFallingFunctionCell.TriggerType.DropEventEnd:                      // Drop event completed (successful or not)
                if (desc.permission == true)                                    // If drop successful (was permitted before)
                {
                    Debug.Log("Successful drop " + desc.item.name + " to " + destinationSheet.name);
                }
                else                                                            // If drop unsuccessful (was denied before)
                {
                    Debug.Log("Denied drop " + desc.item.name + " to " + destinationSheet.name);
                }
                break;
            case DragAndDropFallingFunctionCell.TriggerType.ItemAdded:                         // New item is added from application
                Debug.Log("Item " + desc.item.name + " added into " + destinationSheet.name);
                break;
            case DragAndDropFallingFunctionCell.TriggerType.ItemWillBeDestroyed:               // Called before item be destructed (can not be canceled)
                Debug.Log("Item " + desc.item.name + " will be destroyed " );
                break;
            default:
                Debug.Log("Unknown drag and drop event");
                break;
        }
    }



}
