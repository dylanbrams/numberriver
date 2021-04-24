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
        DragAndDropController destinationSheet = desc.DestinationCell.GetComponentInParent<DragAndDropController>();
        switch (desc.TriggerType)                                               // What type event is?
        {
            case DragAndDropFallingFunctionCell.TriggerType.DropRequest:                       // Request for item drag (note: do not destroy item on request)
                Debug.Log("Request " + desc.Item.name + " to " + destinationSheet.name);
                break;
            case DragAndDropFallingFunctionCell.TriggerType.DropEventEnd:                      // Drop event completed (successful or not)
                if (desc.Permission == true)                                    // If drop successful (was permitted before)
                {
                    Debug.Log("Successful drop " + desc.Item.name + " to " + destinationSheet.name);
                }
                else                                                            // If drop unsuccessful (was denied before)
                {
                    Debug.Log("Denied drop " + desc.Item.name + " to " + destinationSheet.name);
                }
                break;
            case DragAndDropFallingFunctionCell.TriggerType.ItemAdded:                         // New item is added from application
                Debug.Log("Item " + desc.Item.name + " added into " + destinationSheet.name);
                break;
            case DragAndDropFallingFunctionCell.TriggerType.ItemWillBeDestroyed:               // Called before item be destructed (can not be canceled)
                Debug.Log("Item " + desc.Item.name + " will be destroyed " );
                break;
            default:
                Debug.Log("Unknown drag and drop event");
                break;
        }
    }



}
