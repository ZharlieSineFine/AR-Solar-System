using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 _offset;              // Offset from the mouse position to the piece's position
    private Vector2 _originalPosition;     // Original position of the piece
    private bool _isInSlot;                // Track if the piece is in a slot

    public string pieceID;                 // Unique identifier for the puzzle piece
    public float radius = 50f;

    void Start()
    {
        // Generate a unique identifier for this puzzle piece
        //pieceID = "Earth"; // Assign based on the specific piece type
        _originalPosition = transform.position; // Store the original position
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isInSlot) return; // Prevent dragging if the piece is already in a slot

        RectTransform rectTransform = GetComponent<RectTransform>();
        _offset = (Vector2)rectTransform.position - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isInSlot) return; // Prevent dragging if the piece is already in a slot

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.position = eventData.position + _offset; // Update position
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag initiated.");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        bool placedInSlot = false;

        foreach (var hitCollider in hitColliders)
        {
            Debug.Log($"{hitCollider} loop");
            var slot = hitCollider.gameObject.GetComponent<PuzzleSlot>();
            //Debug.Log($"slot {slot.slotID}");
            if (slot != null && slot.slotID == pieceID) // Check if IDs match
            {
                Debug.Log($"{pieceID} is in {slot.slotID}");
                slot.PlacePiece(this); // Place the piece in the slot
                placedInSlot = true;   
                _isInSlot = true;      
                break;                 
            }
        }

        Debug.Log($"Place in slot {placedInSlot}");

        // Reset position if not placed in a slot
        if (!placedInSlot)
        {
            transform.position = _originalPosition; // Return to original position
        }
    }

    public void SetInSlot(bool isInSlot)
    {
        _isInSlot = isInSlot; // Update the in-slot status
    }

    public void ResetPosition()
    {
        transform.position = _originalPosition; // Reset to original position
    }
}