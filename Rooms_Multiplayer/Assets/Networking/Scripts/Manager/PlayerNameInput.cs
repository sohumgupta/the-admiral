using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle player name input
/// </summary>
public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    private const string PlayerPrefsNameKey = "PlayerName";

    /// <summary>
    /// Method called when gameobject in first frame update
    /// </summary>
    void Start()
    {
        SetUpInputField();
    }

    /// <summary>
    /// Method to get previous name if available
    /// </summary>
    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    /// <summary>
    /// Method to set interactability of continueButton
    /// </summary>
    /// <param name="name">Current name</param>
    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    /// <summary>
    /// Method to save player name
    /// </summary>
    public void SavePlayerName()
    {
        string playerName = nameInputField.text;

        PhotonNetwork.NickName = playerName;

        PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
    }

    void Update()
    {
        if (nameInputField.text.Length > 15)
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = !string.IsNullOrEmpty(nameInputField.text);
        }
    }
}
