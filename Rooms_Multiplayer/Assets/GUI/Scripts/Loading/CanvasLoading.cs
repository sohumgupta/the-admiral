using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class for the loading screen.
/// </summary>
public class CanvasLoading : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI funFactText;
    [SerializeField] private TextMeshProUGUI loadingStatusText;
    [SerializeField] private Image loadingBarImage;

    List<string> funFacts =
        new List<string>(new string[] {
            "Fun Fact: Modern day submarines are powered by nuclear reactors!",
            "Fun Fact: Submarines are a special type of watercraft that can operate underwater.",
            "Fun Fact: The average cost of a submarine is $30 million a year. Aren't you glad this game is free?",
            "Fun Fact: Some submarines can remain submerged for months at a time. There goes my summer vacation!",
            "Fun Fact: Any group of fish that stay together for social reasons is said to be shoaling. Would you like to be my shoalmate?",
            "Pro Tip: If you're the Admiral, don't tell anyone!",
            "Pro Tip: Mines do damage, so steer clear of them!",
            "Pro Tip: Don't click the x button if you don't want the game to break!"});

    private int numPlayersReady;
    // Start is called before the first frame update
    void Start()
    {
        numPlayersReady = 0;
        StartCoroutine(LoadTerrain());
        int randomIndex = Random.Range(0, funFacts.Count);
        funFactText.text = funFacts[randomIndex];
    }

    /// <summary>
    /// Method to load the terrain.
    /// </summary>
    IEnumerator LoadTerrain()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // PhotonNetwork.LoadLevel("TimScene");
            PhotonNetwork.LoadLevel("TimScene");
        }

        while(PhotonNetwork.LevelLoadingProgress < 1)
        {
            loadingBarImage.fillAmount = PhotonNetwork.LevelLoadingProgress;
            yield return new WaitForEndOfFrame();
        }
    }
}
