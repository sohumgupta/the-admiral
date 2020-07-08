using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class that dictates the behavior for player collision.
/// </summary>
public class PlayerCollision : MonoBehaviourPun
{
    public GameObject explosion;

    /// <summary>
    /// Method that controls behavior when player hits a mine or a wall.
    /// </summary>
    /// <param name="hit"></param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        PlayerHealth health = FindObjectOfType<PlayerHealth>();
        PlayerMovement player = FindObjectOfType<PlayerMovement>();

        if (hit.transform.CompareTag("Mine"))
        {
            health.TakeDamage(30f * Time.deltaTime);

            Destroy(hit.transform.gameObject);

            GameObject currentExplosion = Instantiate(explosion, hit.transform.position, Quaternion.identity);
            Destroy(currentExplosion, 1.5f);
        }
        else
        {
            health.TakeDamage(0.5f * Time.deltaTime);
        }
    }
}
