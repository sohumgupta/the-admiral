using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to control the behavior of mines.
/// </summary>
public class MineBehavior : MonoBehaviour
{
    private float rotateSpeed = .25f;
    private float lastUpdated;
    Quaternion to;

    // Start is called before the first frame update
    void Start()
    {
        lastUpdated = 0;
        to = Quaternion.Euler(new Vector3(
            Random.Range(-180.0f, 180.0f), 
            Random.Range(-180.0f, 180.0f),
            Random.Range(-180.0f, 180.0f)));
    }

    // Update is called once per frame
    void Update()
    {
        if (lastUpdated > 2f)
        {
            to = Quaternion.Euler(new Vector3(
            Random.Range(-180.0f, 180.0f),
            Random.Range(-180.0f, 180.0f),
            Random.Range(-180.0f, 180.0f)));
            lastUpdated = 0f;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, to, Time.deltaTime * rotateSpeed);
        lastUpdated += Time.deltaTime;
    }
}
