using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that determines behavior of the platform under the treasure chest.
/// </summary>
public class PlatformMovement : MonoBehaviour
{
    PlayerMovement player;
    private float topSpeed = 2000f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        transform.Rotate(Vector3.forward, (1 / distance) * Time.deltaTime * topSpeed);
    }
}
