using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the the Minimap camera.
/// </summary>
public class MinimapCamera : MonoBehaviour
{
    public PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            player.transform.position.x,
            30,
            player.transform.position.z);
        transform.rotation = Quaternion.Euler(90, player.transform.localEulerAngles.y, 0);
    }
}
