using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that determines the behavior of the submarine's movement.
/// </summary>
public class PlayerMovement : MonoBehaviourPun
{
    private Transform selfTransform;

    float velocity;

    float throttleMax = 25f;
    float throttleRate = 1.5f;

    float yawRate = 50f;
    float pitchRate = 20f;

    float slowRate = .98f;
    float EPSILON = .05f;

    public bool canMove;
    bool isMoving = false;

    public Vector3 playerPosition;
    public Quaternion playerRotation;
    float offsetScale = 70;

    private CharacterController controller;

    /// <summary>
    /// Method to update the player's position.
    /// </summary>
    void updatePos() {
        playerPosition = transform.position;
    }

    void Start()
    {
        selfTransform = transform;
        velocity = 0;
        canMove = false;
        controller = FindObjectOfType<CharacterController>();
    }

    /// <summary>
    /// Method to get the velocity.
    /// </summary>
    /// <returns>velocity as a float.</returns>
    public float getVelocity()
    {
        return velocity;
    }

    /// <summary>
    /// Method to move the submarine based on yaw, pitch, and throttle.
    /// </summary>
    private void Move()
    {
        float yawInput = Input.GetAxis("Yaw");
        float pitchInput = Input.GetAxis("Pitch");
        float throttleInput = Input.GetAxis("Throttle");

        if (throttleInput > 0)
        {
            if (velocity <= EPSILON) {
                velocity += throttleRate;
            } else if (velocity < throttleMax) {
                velocity *= throttleRate;
            }
        }
        else
        {
            if (velocity > 0) {
                if (velocity <= EPSILON) {
                    velocity = 0;
                } else {
                    velocity *= slowRate;
                }
            }
        }

        if (velocity > throttleMax) { velocity = throttleMax; }
        if (velocity < 0) { velocity = 0; }

        transform.Rotate(Time.deltaTime * pitchInput * pitchRate, Time.deltaTime * yawInput * yawRate, 0);
        Vector3 myRotation = transform.rotation.eulerAngles;
        myRotation.z = 0;
        transform.rotation = Quaternion.Euler(myRotation);

        controller.Move(transform.forward * velocity * Time.deltaTime);
    }

    void Update()
    {
        if (photonView.IsMine && canMove)
        {
            Move();
        }
        updatePos();
    }
}
