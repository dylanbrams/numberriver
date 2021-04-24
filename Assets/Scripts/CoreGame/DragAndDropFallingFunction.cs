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
	public static bool DragDisabled = false;                                        // Drag start global disable

	public static GameObject DraggedParticle;
	public static DragAndDropFallingFunction DraggedItem;                                      // Item that is dragged now
	public static DragAndDropFallingFunctionCell SourceCell;                                       // From this cell dragged item is
	public static bool DraggingActive = false;

	public delegate void DragEvent(DragAndDropFallingFunction item);
	public static event DragEvent OnItemDragStartEvent;                             // Drag start event
	public static event DragEvent OnItemDragEndEvent;                               // Drag end event

	[SerializeField] Canvas canvas;                                                   // Canvas for item drag operation
	private static string canvasName = "DragAndDropCanvas";                         // Name of canvas
	private static int canvasSortOrder = 100;                                       // Sort order for canvas

	private Vector2 _previousMovement;
	private Vector2 _previousConstantForce;
	private Rigidbody2D _myRigidBody2D;
	private ConstantForce2D _constantForce2D;
	private FallingFunction _myFallingFunction;
	private bool _movementStopped = false;
	public bool movementHalted = false;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		canvas = gameObject.GetComponentInParent<Canvas>();
		_myRigidBody2D = gameObject.GetComponent<Rigidbody2D>();
		_constantForce2D = gameObject.GetComponent<ConstantForce2D>();
		_myFallingFunction = gameObject.GetComponent<FallingFunction>();
	}

	/// <summary>
	/// This item started to drag.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag(PointerEventData eventData)
	{
		Debug.Log("BeginDrag");
		if (DragDisabled == false)
		{
			DraggingActive = true;
			SourceCell = GetCell();                                                 // Remember source cell
			DraggedItem = this;                                                     // Set as dragged item
																					// Create item's icon

			DraggedParticle = CreateParticleSystemInCanvas(draggingParticlePrefab, DraggedItem.transform);
			DraggedParticle.name = "Particles";

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
		if (DraggedParticle != null)
		{
			Vector3 cameraPos = Camera.main.ScreenToWorldPoint((Input.mousePosition));                          // Item's icon follows to cursor in screen pixels
			DraggedParticle.transform.position = new Vector3(cameraPos.x, cameraPos.y);
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
		if (DraggedParticle)
        {
			finishDragParticles = CreateParticleSystemInCanvas(finishDragPrefab, DraggedParticle.transform);
        }
		else
			finishDragParticles = CreateParticleSystemInCanvas(finishDragPrefab, transform);
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
		if (DraggedParticle != null)
		{
			Destroy(DraggedParticle);                                                          // Destroy icon on item drop
		}
		DraggedItem = null;
		DraggedParticle = null;
		SourceCell = null;
		DraggingActive = false;
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

	private GameObject CreateParticleSystemInCanvas(GameObject particlePrefab, Transform inputTransform)
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
		if (!_movementStopped || movementHalted)
		{
			_movementStopped = true;
			_previousConstantForce = _constantForce2D.force;
			_constantForce2D.force = new Vector2(0, 0);
			_previousMovement = _myRigidBody2D.velocity;
			_myRigidBody2D.velocity = new Vector2(0, 0);
		}
    }

	public void RestartMovement()
    {
		if (!movementHalted)
        {
			_constantForce2D.force = _previousConstantForce;
			_myRigidBody2D.velocity = _previousMovement;
			_movementStopped = false;
		}
	}

	public void HaltMovement()
    {
		movementHalted = true;
    }

	public bool CanMove()
	{
		if (!movementHalted && !_movementStopped)
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
		_myFallingFunction.ProcessCombine(functionIn);
    }
}
