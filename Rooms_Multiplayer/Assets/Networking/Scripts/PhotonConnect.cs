using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Method for handling connection to server
/// </summary>
public class PhotonConnect : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Method triggered when entering the game
    /// </summary>
    public void OnClick_ConnectToServer()
    {
        print("Connecting to server...");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Callback for when connected to master, joins lobby
    /// </summary>
    public override void OnConnectedToMaster()
    {
        print("Connecting to server");
        if (!PhotonNetwork.InLobby)
        {
            print("Not in lobby, joining lobby...");
            PhotonNetwork.JoinLobby();
        }
    }

    /// <summary>
    /// Callback when connection failed or disconnected
    /// </summary>
    /// <param name="cause">Cause of disconnect</param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server for reason " + cause.ToString());
    }
}
