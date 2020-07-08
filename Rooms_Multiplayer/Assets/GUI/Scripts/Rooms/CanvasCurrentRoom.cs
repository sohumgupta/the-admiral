using UnityEngine;

/// <summary>
/// Class for the CurrentRoom
/// </summary>
public class CanvasCurrentRoom : MonoBehaviour
{
    [SerializeField] private PlayerListingsMenu playerListingsMenu;
    [SerializeField] private LeaveRoomMenu leaveRoomMenu;
    public LeaveRoomMenu LeaveRoomMenu {  get { return leaveRoomMenu; } }

    private CanvasesRooms canvasesRooms;

    /// <summary>
    /// Method to get a reference to other canvases
    /// </summary>
    /// <param name="canvases">Reference to all canvases</param>
    public void Initialize(CanvasesRooms canvases)
    {
        canvasesRooms = canvases;
        playerListingsMenu.Initialize(canvases);
        leaveRoomMenu.Initialize(canvases);
    }

    /// <summary>
    /// Method to show this canvas
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Method to hide this canvas
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
