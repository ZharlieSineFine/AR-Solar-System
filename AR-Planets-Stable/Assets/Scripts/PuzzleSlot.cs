using UnityEngine;
using UnityEngine.UI; // For UI elements

public class PuzzleSlot : MonoBehaviour
{
    public Image Renderer; // Reference to the Image component for visual representation
    private PuzzlePiece currentPiece; // Track the currently placed puzzle piece
    public PuzzleManager puzzleManager; // Reference to the PuzzleManager

    public string slotID; // Unique identifier for the puzzle slot

    void Start()
    {
        // Assign a unique identifier for this puzzle slot
        //slotID = "Earth"; // Example: Assign based on the specific slot type
        Debug.Log("Initialized Slot ID: " + slotID); // Log the slot ID on initialization
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Highlight the slot when a piece is near (if using triggers)
        if (other.CompareTag("PuzzlePiece"))
        {
            var piece = other.GetComponent<PuzzlePiece>();
            if (piece != null)
            {
                Debug.Log($"Piece is {piece}");
                Renderer.color = Color.green; // Highlight the slot
                Debug.Log($"Piece {piece.pieceID} entered slot {slotID}. Highlighting slot."); // Log entry
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Reset color when piece exits (if using triggers)
        if (other.CompareTag("PuzzlePiece"))
        {
            Renderer.color = Color.white; // Reset color
            Debug.Log($"Piece exited slot {slotID}. Resetting slot color."); // Log exit
        }
    }

    public void PlacePiece(PuzzlePiece piece)
    {
        if (piece == null)
        {
            Debug.LogWarning("Attempted to place a null piece."); // Log warning
            return; // Safety check
        }

        // Snap the piece to the slot's position
        currentPiece = piece;
        piece.transform.position = transform.position; // Snap to slot position

        // Update the slot image with the piece's sprite
        Image pieceImage = piece.GetComponent<Image>();
        if (pieceImage != null)
        {
            Renderer.sprite = pieceImage.sprite; // Update the Image component's sprite
            Debug.Log($"Slot {slotID} updated with piece sprite from {piece.pieceID}."); // Log sprite update
        }
        else
        {
            Debug.LogWarning($"Piece {piece.pieceID} does not have an Image component."); // Log warning if no image
        }

        // Notify PuzzleManager that a piece has been placed
        PuzzleManager.Instance.PiecePlaced(piece.pieceID, slotID); // Correctly passing IDs
        Debug.Log($"Piece {piece.pieceID} placed in slot {slotID}."); // Log placement

        // Set the piece as being in a slot
        piece.SetInSlot(true);
    }

    // Optionally, implement a method to clear the slot if needed
    public void ClearSlot()
    {
        currentPiece = null;
        Renderer.sprite = null; // Reset the slot image if necessary
        Debug.Log($"Slot {slotID} cleared."); // Log clearing the slot
    }
}