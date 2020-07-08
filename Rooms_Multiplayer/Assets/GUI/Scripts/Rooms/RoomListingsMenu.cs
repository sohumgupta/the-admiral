using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for all current room listings
/// </summary>
public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private RoomListing roomListing;
    [SerializeField] private Transform content;

    private List<RoomListing> listings = new List<RoomListing>();
    public CanvasesRooms canvasesRooms;

    /// <summary>
    /// Method to get a reference to other canvases
    /// </summary>
    /// <param name="canvases">Reference to all canvases</param>
    public void Initialize(CanvasesRooms canvases)
    {
        print("Initialize RoomListingMenu");
        if (canvases == null)
        {
            Debug.LogError("Canvases is null");
        }
        canvasesRooms = canvases;
    }

    /// <summary>
    /// Callback for when one joins a room, hide and show canvases and delete room listings
    /// </summary>
    public override void OnJoinedRoom()
    {
        //if(canvasesRooms == null)
        //{
        //    Debug.LogError("no canvases");
        //}
        //if (canvasesRooms.CanvasCreateOrJoinRoom == null)
        //{
        //    Debug.LogError("no create room");
        //}
        //if (canvasesRooms.CanvasCurrentRoom == null)
        //{
        //    Debug.LogError("no current room");
        //}
        canvasesRooms.CanvasCreateOrJoinRoom.Hide();
        canvasesRooms.CanvasCurrentRoom.Show();
        content.DestroyChildren();
        listings.Clear();
        PhotonNetwork.KeepAliveInBackground = 600f;
    } 

    /// <summary>
    /// Callback for when room list has updated, update listings accordingly
    /// </summary>
    /// <param name="roomList">List of rooms</param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if(info.RemovedFromList)
            {
                int index = listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(listings[index].gameObject);
                    listings.RemoveAt(index);
                }
            }
            else
            {
                int index = listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    RoomListing listing = Instantiate(roomListing, content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        listings.Add(listing);
                    }
                } 
                
            }
        }
    }

}
