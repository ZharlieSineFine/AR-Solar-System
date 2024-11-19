using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; } // Singleton instance
    private int totalPieces; // Total number of puzzle pieces
    private int piecesPlaced; // Pieces currently placed in slots

    void Awake()
    {
        // Ensure that there's only one instance of PuzzleManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persists across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }

    void Start()
    {
        // Initialize total pieces based on PuzzlePiece objects in the scene
        totalPieces = FindObjectsOfType<PuzzlePiece>().Length;
        piecesPlaced = 0;

        Debug.Log($"Total Pieces: {totalPieces}"); // Log total pieces for debugging
    }

    public void PiecePlaced(string pieceID, string slotID)
    {
        piecesPlaced++;
        Debug.Log($"Piece {pieceID} placed in slot {slotID}. Total Placed: {piecesPlaced}/{totalPieces}"); // Log placement

        CheckCompletion();
    }

    private void CheckCompletion()
    {
        if (piecesPlaced >= totalPieces)
        {
            Debug.Log("Puzzle Completed!");
            ShowVictoryUI(); // Trigger victory UI or reset option
        }
    }

    public void ResetPuzzle()
    {
        piecesPlaced = 0;
        Debug.Log("Puzzle Reset!"); // Log for debugging

        // Reset positions for all puzzle pieces
        foreach (var piece in FindObjectsOfType<PuzzlePiece>())
        {
            piece.ResetPosition(); // Reset each piece to its original position
            piece.SetInSlot(false); // Ensure in-slot status is reset
        }

        // Optionally, clear any slots if needed
        foreach (var slot in FindObjectsOfType<PuzzleSlot>())
        {
            slot.ClearSlot(); // Clear each slot if you implement this method
        }
    }

    private void ShowVictoryUI()
    {
        // Implement UI Logic here to show victory screen
        Debug.Log("Showing Victory UI...");
    }
}