using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Class for roles of all players
/// </summary>
public class AllCurrentRoles : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI currentPilotText;
    [SerializeField] private TextMeshProUGUI currentNavigatorText;
    [SerializeField] private TextMeshProUGUI currentCaptainText;

    /// <summary>
    /// Method to set the pilot text
    /// </summary>
    /// <param name="nickname">Nickname of the player</param>
    private void SetCurrentPilotText(string nickname)
    {
        string role = "";
        role += nickname;
        currentPilotText.text = role;
    }

    /// <summary>
    /// Method to set the navigator text
    /// </summary>
    /// <param name="nickname">Nickname of the player</param>
    private void SetCurrentNavigatorText(string nickname)
    {
        string role = "";
        role += nickname;
        currentNavigatorText.text = role;
    }

    /// <summary>
    /// Method to set the captain text
    /// </summary>
    /// <param name="nickname">Nickname of the player</param>
    private void SetCurrentCaptainText(string nickname)
    {
        string role = "";
        role += nickname;
        currentCaptainText.text = role;
    }

    /// <summary>
    /// Callback for when player properties are updated
    /// </summary>
    /// <param name="targetPlayer">Player whos properties were updated</param>
    /// <param name="changedProps">Changed properties</param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer != null)
        {
            if (changedProps.ContainsKey("Role"))
            {
                string roleChanged = (string)changedProps["Role"];
                string nickname = targetPlayer.NickName;
                if (roleChanged.Equals("Pilot"))
                {
                    SetCurrentPilotText(nickname);
                } else if (roleChanged.Equals("Navigator")) {
                    SetCurrentNavigatorText(nickname);
                } else if (roleChanged.Equals("Captain"))
                {
                    SetCurrentCaptainText(nickname);
                }
            }
        }
    }
}
