using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using TMPro;

/// <summary>
/// Drag and Drop item.
/// </summary>
public class DragAndDropFallingFunction : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] GameObject draggingParticlePrefab;
	[SerializeField] GameObject finishDragPrefab;
	[SerializeField] float finishDragParticlesDestroyTime = 5;
	public static bool dragDisabled = false;                                        // Drag start global disable

	public static GameObject draggedParticle;
	public static DragAndDropFallingFunction draggedItem;                                      // Item that is dragged now
	public static DragAndDropFallingFunctionCell sourceCell;                                       // From this cell dragged item is
	public static bool draggingActive = false;

	public delegate void DragEvent(DragAndDropFallingFunction item);
	public static event DragEvent OnItemDragStartEvent;                             // Drag start event
	public static event DragEvent OnItemDragEndEvent;                               // Drag end event

	[SerializeField] Canvas canvas;                                                   // Canvas for item drag operation
	private static string canvasName = "DragAndDropCanvas";                         // Name of canvas
	private static int canvasSortOrder = 100;                                       // Sort order for canvas

	private Vector2 previousMovement;
	private Vector2 previousConstantForce;
	private Rigidbody2D myRigidBody2D;
	private ConstantForce2D constantForce2D;
	private FallingFunction myFallingFunction;
	private bool movementStopped = false;
	public bool movementHalted = false;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		canvas = gameObject.GetComponentInParent<Canvas>();
		myRigidBody2D = gameObject.GetComponent<Rigidbody2D>();
		constantForce2D = gameObject.GetComponent<ConstantForce2D>();
		myFallingFunction = gameObject.GetComponent<FallingFunction>();
	}

	/// <summary>
	/// This item started to drag.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag(PointerEventData eventData)
	{
		Debug.Log("BeginDrag");
		if (dragDisabled == false)
		{
			draggingActive = true;
			sourceCell = GetCell();                                                 // Remember source cell
			draggedItem = this;                                                     // Set as dragged item
																					// Create item's icon

			draggedParticle = createParticleSystemInCanvas(draggingParticlePrefab, draggedItem.transform);
			draggedParticle.name = "Particles";

			if (OnItemDragStartEvent != null)
			{
				OnItemDragStartEvent(this);                                         // Notify all items about drag start for raycast disabling
			}
		}
	}

	/// <summary>
	/// Every frame on this item drag.
	/// </summary>
	/// <param name="data"></param>
	public void OnDrag(PointerEventData data)
	{
		if (draggedParticle != null)
		{
			Vector3 cameraPos = Camera.main.ScreenToWorldPoint((Input.mousePosition));                          // Item's icon follows to cursor in screen pixels
			draggedParticle.transform.position = new Vector3(cameraPos.x, cameraPos.y);
			StopMovement();
			/* Debug.Log("Mouse Position: " + Input.mousePosition.ToString() + " Particle Position: " + draggedParticle.transform.position.ToString() +
				" Dragged Item Name: " + draggedItem.name.ToString() + " Dragged Item Position: " + draggedItem.transform.position.ToString() +
				" Dragged item scale: " + draggedItem.transform.localScale.ToString()); */
		}
	}

	/// <summary>
	/// This item is dropped.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("Drag Ended");
		GameObject finishDragParticles;
		if (draggedParticle)
        {
			finishDragParticles = createParticleSystemInCanvas(finishDragPrefab, draggedParticle.transform);
        }
		else
			finishDragParticles = createParticleSystemInCanvas(finishDragPrefab, transform);
		ResetConditions();
		Destroy(finishDragParticles, finishDragParticlesDestroyTime);
	}

	/// <summary>
	/// Resets all temporary conditions.
	/// </summary>
	private void ResetConditions()
	{
		if (OnItemDragEndEvent != null)
		{
			OnItemDragEndEvent(this);                                               // Notify all cells about item drag end
		}
		if (draggedParticle != null)
		{
			Destroy(draggedParticle);                                                          // Destroy icon on item drop
		}
		draggedItem = null;
		draggedParticle = null;
		sourceCell = null;
		draggingActive = false;
		RestartMovement();
	}

	/// <summary>
	/// Enable item's raycast.
	/// </summary>
	/// <param name="condition"> true - enable, false - disable </param>
	public void MakeRaycast(bool condition)
	{
		Image image = GetComponent<Image>();
		if (image != null)
		{
			image.raycastTarget = condition;
		}
	}

	private GameObject createParticleSystemInCanvas(GameObject particlePrefab, Transform inputTransform)
    {
		GameObject myObj = Instantiate(particlePrefab, inputTransform);
		myObj.transform.SetParent(canvas.transform);
		myObj.transform.localScale = inputTransform.localScale;
		return myObj;
	}

	/// <summary>
	/// Gets DaD cell which contains this item.
	/// </summary>
	/// <returns>The cell.</returns>
	public DragAndDropFallingFunctionCell GetCell()
	{
		return GetComponentInParent<DragAndDropFallingFunctionCell>();
	}

	public void StopMovement()
    {
		if (!movementStopped || movementHalted)
		{
			movementStopped = true;
			previousConstantForce = constantForce2D.force;
			constantForce2D.force = new Vector2(0, 0);
			previousMovement = myRigidBody2D.velocity;
			myRigidBody2D.velocity = new Vector2(0, 0);
		}
    }

	public void RestartMovement()
    {
		if (!movementHalted)
        {
			constantForce2D.force = previousConstantForce;
			myRigidBody2D.velocity = previousMovement;
			movementStopped = false;
		}
	}

	public void HaltMovement()
    {
		movementHalted = true;
    }

	public bool CanMove()
	{
		if (!movementHalted && !movementStopped)
		{
			return true;
		}
		else return false;
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		ResetConditions();
	}

	public void ProcessCombine(FallingFunction functionIn)
    {
		myFallingFunction.ProcessCombine(functionIn);
    }
}
