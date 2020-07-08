using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for icon movement.
/// </summary>
public class IconMovement : MonoBehaviour
{
    private Transform parent;

    private void Start()
    {
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(90, parent.transform.localEulerAngles.y, 0);
    }
}
