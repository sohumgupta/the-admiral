using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the Compass.
/// </summary>
public class Compass : MonoBehaviour
{
    private PlayerMovement player;
    private TreasureBehavior treasure;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        treasure = FindObjectOfType<TreasureBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        var target_pos_local = player.transform.InverseTransformPoint(treasure.transform.position);
        var angle_target_h = Mathf.Atan2(target_pos_local.x, target_pos_local.z) * Mathf.Rad2Deg;

        this.transform.eulerAngles = new Vector3(0, 0, -angle_target_h);
    }
}
