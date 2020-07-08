using Photon.Pun;
using UnityEngine;

/// <summary>
/// Method to handle leaving room
/// </summary>
public class LeaveRoomMenu : MonoBehaviour
{
    private CanvasesRooms canvasesRooms;

    /// <summary>
    /// Method to get a reference to other canvases
    /// </summary>
    /// <param name="canvases">Reference to all canvases</param>
    public void Initialize(CanvasesRooms canvases)
    {
        canvasesRooms = canvases;
    }

    /// <summary>
    /// Triggered when leave room button is clicked, hides and shows relevant canvases
    /// </summary>
    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        canvasesRooms.CanvasCurrentRoom.Hide();
        canvasesRooms.CanvasCreateOrJoinRoom.Show();
    }

    
}
