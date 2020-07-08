using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that controls behavior for player health.
/// </summary>
public class PlayerHealth : MonoBehaviourPun
{
    private float startingHealth = 100f;
    private float currentHealth;

    [SerializeField]
    private Image healthBarFill;
    private Multiplayer.GameManager gameManager;
    private float fullColor = 120f;
    private float emptyColor = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        gameManager = FindObjectOfType<Multiplayer.GameManager>();
    }

    /// <summary>
    /// Method that updates the player's health given the damage.
    /// </summary>
    /// <param name="damage">damage as a float.</param>
    public void TakeDamage(float damage)
    {
        if (photonView.IsMine)
        {
            currentHealth -= damage;
            photonView.RPC("RPC_TakeDamage", RpcTarget.All, currentHealth);
        }
    }

    /// <summary>
    /// Method that updates the player's health bar and determines color based on current health.
    /// </summary>
    /// <param name="newHealth">new health after taking some damage.</param>
    [PunRPC]
    private void RPC_TakeDamage(float newHealth)
    {
        currentHealth = newHealth;
        if (currentHealth <= 0 && PhotonNetwork.IsMasterClient) { gameManager.GameOver = true; }
        healthBarFill.fillAmount = (currentHealth / startingHealth);
    }
}
