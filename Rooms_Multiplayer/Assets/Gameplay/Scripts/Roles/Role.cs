using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Class to handle text changes for local role and team
/// </summary>
public class Role : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI roleText;
    [SerializeField] private TextMeshProUGUI teamText;

    /// <summary>
    /// Set role and team on first frame update
    /// </summary>
    void Start()
    {
        Debug.Log("no assigned role assigned");
        SetRoleText(PhotonNetwork.LocalPlayer);
        SetTeamText(PhotonNetwork.LocalPlayer);
    }

    /// <summary>
    /// Method to set the role text of the local player
    /// </summary>
    /// <param name="player">Local player</param>
    private void SetRoleText(Player player)
    {
        string role = "Current Role";
        if (player.CustomProperties.ContainsKey("Role"))
        {
            Debug.Log("Name change success");
            role = (string)player.CustomProperties["Role"];
        }
        roleText.text = role;
    }

    /// <summary>
    /// Method to set the team text of the local player
    /// </summary>
    /// <param name="player">Local player</param>
    private void SetTeamText(Player player)
    {
        string team = "Allegiance";
        if (player.CustomProperties.ContainsKey("Team"))
        {
            team = (string)player.CustomProperties["Team"];
        }
        teamText.text = team;
    }

    /// <summary>
    /// Callback for when player properties are updated
    /// </summary>
    /// <param name="targetPlayer">Player whos properties were updated</param>
    /// <param name="changedProps">Changed properties</param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer != null && targetPlayer.IsLocal)
        {
            if (changedProps.ContainsKey("Role"))
            {
                SetRoleText(targetPlayer);
            }
            if (changedProps.ContainsKey("Team"))
            {
                SetTeamText(targetPlayer);
            }
        }
    }

}
