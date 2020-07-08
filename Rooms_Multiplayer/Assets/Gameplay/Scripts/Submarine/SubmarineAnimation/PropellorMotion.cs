using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that determines the behavior of the propeller on the back of the submarine.
/// </summary>
public class PropellorMotion : MonoBehaviour
{
    PlayerMovement player;
    private float propellorSpeed = 60f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left, player.getVelocity() * Time.deltaTime * propellorSpeed);
    }
}
