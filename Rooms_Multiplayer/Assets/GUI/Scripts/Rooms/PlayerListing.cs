using Photon.Realtime;
using TMPro;
using UnityEngine;

/// <summary>
/// Class for a player listing
/// </summary>
public class PlayerListing : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public Player Player { get; private set; }
    public bool Ready = false;

    /// <summary>
    /// Method to set the player info of the prefab text
    /// </summary>
    /// <param name="playerInput">The player we want to set info for</param>
    public void SetPlayerInfo(Player playerInput)
    {
        Player = playerInput;
        if (playerInput.IsMasterClient)
        {
            text.text = "HOST: " + playerInput.NickName;
        } else
        {
            text.text = playerInput.NickName;
        }
    }
}
