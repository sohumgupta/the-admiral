using Multiplayer;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to control the movement of the submarine.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviourPun
{
    [SerializeField] private float movementSpeed = 0f;
    private CharacterController controller = null;
    private Transform mainCameraTransform = null;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            TakeInput();
        }
    }

    /// <summary>
    /// Method that updates the position of the submarine given the players position and rotation.
    /// </summary>
    private void TakeInput()
    {
        Vector3 movement = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical"),
        }.normalized;

        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 calculatedMovement = (forward * movement.z + right * movement.x).normalized;

        if(calculatedMovement != Vector3.zero )
        {
            transform.rotation = Quaternion.LookRotation(calculatedMovement);
        }
            

        controller.SimpleMove(calculatedMovement * movementSpeed);
    }
}

