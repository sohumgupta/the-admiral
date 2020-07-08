using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

/// <summary>
/// Class for creating room
/// </summary>
public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private Button createRoomButton = null;

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
    /// Method to create a room
    /// </summary>
    public void OnClick_CreateRoom()
    {
        if(!PhotonNetwork.IsConnected) { return; }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, TypedLobby.Default);
    }

    /// <summary>
    /// Callback for when room is created
    /// </summary>
    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully");
        //canvasesRooms.CanvasCurrentRoom.Show();
    }

    /// <summary>
    /// Callback for when creation of room failed
    /// </summary>
    /// <param name="returnCode">return code of failure</param>
    /// <param name="message">failure message</param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed");
    }

    void Update()
    {
        if (roomName.text.Length > 25)
        {
            createRoomButton.interactable = false;
        }
        else
        {
            createRoomButton.interactable = !string.IsNullOrEmpty(roomName.text);
        }
    }
}
