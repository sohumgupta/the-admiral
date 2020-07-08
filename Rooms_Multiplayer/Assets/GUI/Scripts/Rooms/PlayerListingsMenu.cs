using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// Class for listing all the players in room
/// </summary>
public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerListing playerListing;
    [SerializeField] private Transform content;
    [SerializeField] private TextMeshProUGUI readyUpText;
    [SerializeField] private Toggle readyUpButton;
    [SerializeField] private Button startButton;
    private bool ready = false;
    private CanvasesRooms canvasesRooms;

    private List<PlayerListing> listings = new List<PlayerListing>();

    /// <summary>
    /// Actions for when the game object is set to active
    /// </summary>
    public override void OnEnable()
    {
        base.OnEnable();
        SetReadyUp(false);
        if (PhotonNetwork.IsMasterClient)
        {
            // Only display start button for master client
            startButton.gameObject.SetActive(true);
            readyUpButton.gameObject.SetActive(false);

            // Generate shared seed for room
            int[] seed = generateSeed();
            Hashtable roomProps = new Hashtable();
            roomProps["Seed"] = seed;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
        } else
        {
            // Only display ready button for not master client
            startButton.gameObject.SetActive(false);
            readyUpButton.gameObject.SetActive(true);
        }
        GetCurrentRoomPlayers();
    }

    /// <summary>
    /// Method to generate seed
    /// </summary>
    /// <returns>int[] of the seed</returns>
    public int[] generateSeed()
    {
        System.Random rand = new System.Random();
        int[] seed = new int[8];
        for (int i = 0; i < 8; i++)
        {
            seed[i] = rand.Next();
        }
        return seed;
    }

    /// <summary>
    /// Actions for when game object is set to not active
    /// </summary>
    public override void OnDisable()
    {
        base.OnDisable();
        for(int i = 0; i < listings.Count; i++)
        {
            Destroy(listings[i].gameObject);
        }
        listings.Clear();
    }

    /// <summary>
    /// Method to get a reference to other canvases
    /// </summary>
    /// <param name="canvases">Reference to all canvases</param>
    public void Initialize(CanvasesRooms canvases)
    {
        canvasesRooms = canvases;
    }

    /// <summary>
    /// Method to correctly set ready up text
    /// </summary>
    /// <param name="state"></param>
    private void SetReadyUp(bool state)
    {
        ready = state;
        //if (ready)
        //{
        //    readyUpText.text = "Ready";
        //} else
        //{
        //    readyUpText.text = "Not Ready";
        //}
        
    }

    /// <summary>
    /// Handling left room callback
    /// </summary>
    public override void OnLeftRoom()
    {
        content.DestroyChildren();
    }

    /// <summary>
    /// Get info and display current players
    /// </summary>
    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
        {
            return;
        }
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    /// <summary>
    /// Instantiate prefabs of current players to list
    /// </summary>
    /// <param name="player">Player to add to listing</param>
    private void AddPlayerListing(Player player)
    {
        int index = listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            listings[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(playerListing, content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                listings.Add(listing);
            }
        }
    }

    /// <summary>
    /// Callback for when MasterClient leaves room
    /// </summary>
    /// <param name="newMasterClient">Player reference to new master client</param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        canvasesRooms.CanvasCurrentRoom.LeaveRoomMenu.OnClick_LeaveRoom();
    }

    /// <summary>
    /// Callback to handle new player entering room
    /// </summary>
    /// <param name="newPlayer">reference to new player</param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
        startButton.interactable = false;
    }

    /// <summary>
    /// Callback to handle player leaving room
    /// </summary>
    /// <param name="otherPlayer">reference to player that left</param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = listings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(listings[index].gameObject);
            listings.RemoveAt(index);
        }
    }

    /// <summary>
    /// Method to handle starting the game
    /// </summary>
    public void OnClick_StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            //CHANGE THIS BACK TO LOAD LEVEL 1
            PhotonNetwork.LoadLevel("Scene_Load");
        }
    }

    /// <summary>
    /// Method to handle readying up
    /// </summary>
    public void OnClick_ReadyUp()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            SetReadyUp(!ready);
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, ready);
        }
    }

    /// <summary>
    /// RPC to communicate ready state to master client
    /// </summary>
    /// <param name="player">Player that is readying up</param>
    /// <param name="ready">Status of readyzz</param>
    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            listings[index].Ready = ready;
        }

        for (int i = 0; i < listings.Count; i++)
        {
            if (listings[i].Player != PhotonNetwork.LocalPlayer)
            {
                if (!listings[i].Ready)
                {
                    startButton.interactable = false;
                    return;
                }
            }
        }
        //CHANGE SO NO 2 PLAYER
        if (PhotonNetwork.CurrentRoom.PlayerCount != 1)
        {
            startButton.interactable = true;
        }
    }
}
