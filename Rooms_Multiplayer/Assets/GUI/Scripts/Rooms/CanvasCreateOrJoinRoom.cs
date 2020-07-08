using UnityEngine;

/// <summary>
/// Class for the CreateOrJoinRoom
/// </summary>
public class CanvasCreateOrJoinRoom : MonoBehaviour
{
    [SerializeField] private CreateRoomMenu createRoomMenu;
    [SerializeField] private RoomListingsMenu roomListingsMenu;

    private CanvasesRooms canvasesRooms;

    /// <summary>
    /// Method to get a reference to other canvases
    /// </summary>
    /// <param name="canvases">Reference to all canvases</param>
    public void Initialize(CanvasesRooms canvases)
    {
        canvasesRooms = canvases;
        createRoomMenu.Initialize(canvases);
        roomListingsMenu.Initialize(canvases);
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
