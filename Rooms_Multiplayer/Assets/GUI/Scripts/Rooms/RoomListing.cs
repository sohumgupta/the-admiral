using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

/// <summary>
/// Class for a single room listing
/// </summary>
public class RoomListing : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public RoomInfo RoomInfo { get; private set; }

    /// <summary>
    /// Method to set room info for the prefab
    /// </summary>
    /// <param name="roomInfo">Room information of the room</param>
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        text.text = roomInfo.MaxPlayers + ", " + roomInfo.Name;
    }

    /// <summary>
    /// Method to join a room
    /// </summary>
    public void OnClick_JoinRoom()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }

    
}
