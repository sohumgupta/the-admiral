using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that determines the behavior of the submarine lookout animation.
/// </summary>
public class SubmarineLookoutAnimation : MonoBehaviour
{
    private float speed = 50f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(1, 0, 0), speed * Time.deltaTime);
    }
}
