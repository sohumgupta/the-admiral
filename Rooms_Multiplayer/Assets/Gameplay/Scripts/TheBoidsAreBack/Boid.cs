using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing a boid (fish).
/// </summary>
public class Boid : MonoBehaviour
{
    public int id;

    public Vector3 position;
    Vector3 velocity;

    float rotationAmount = 0.05f;

    float scaleVariance = 0.3f;

    float avoidance = -.01f;

    double maxAcceleration = 1f;

    float yVelClamp = 6f;

    //collisions
    bool canCollide = true;
    public float boundsRadius = 1f;
    public float steerAmount = 10;
    public float collisionAvoidDst = 7;

    /// <summary>
    /// Method that sets the scale of the fish.
    /// </summary>
    /// <param name="scaleMean">the average scale for a boid.</param>
    public void setScale(float scaleMean) {
        if ((scaleMean - scaleVariance) < 0) {
            throw new System.ArgumentException("Scale mean was too low!");
        }
        transform.localScale = transform.localScale * Random.Range(scaleMean - scaleVariance, scaleMean + scaleVariance);
    }

    /// <summary>
    /// Method that changes and updates the position of the boid.
    /// </summary>
    public void changePosition() {
        transform.position += this.velocity * Time.deltaTime;
        this.position = transform.position;
    }

    /// <summary>
    /// Method that changes and updates the rotation of the boid.
    /// </summary>
    public void updateRotation() {
        Quaternion rot = new Quaternion();
        Vector3 euler = new Vector3(0,0,0);
        rot[0] = transform.rotation[0] + rotationAmount * Quaternion.LookRotation(this.velocity)[0];
        rot[1] = transform.rotation[1] + rotationAmount * Quaternion.LookRotation(this.velocity)[1];
        rot[2] = transform.rotation[2] + rotationAmount * Quaternion.LookRotation(this.velocity)[2];
        rot[3] = transform.rotation[3] + rotationAmount * Quaternion.LookRotation(this.velocity)[3];
        transform.rotation = rot;
    }

    /// <summary>
    /// Method to clamp velocities within a certain range to prevent high speed fish
    /// </summary>
    public void clamp() {
        this.velocity = Vector3.ClampMagnitude(this.velocity, 10);
        if (this.velocity[1] > yVelClamp) {
            this.velocity[1] = yVelClamp;
        }
        if (this.velocity[1] < -1 * yVelClamp) {
            this.velocity[1] = -1 * yVelClamp;
        }
    }

    /// <summary>
    /// Method to set the id of the boid.
    /// </summary>
    /// <param name="id">id as an int.</param>
    public void setId(int id) {
        this.id = id;
        
    }

    /// <summary>
    /// Method to get the position of the boid.
    /// </summary>
    /// <returns>the position of the boid as a Vector3.</returns>
    public Vector3 getPosition() {
        return this.position;
    }

    /// <summary>
    /// Method to get the velociy of the boid.
    /// </summary>
    /// <returns>the velocity of the boid as a Vector3.</returns>
    public Vector3 getVelocity() {
        return this.velocity;
    }

    /// <summary>
    /// Method to update the velocity of the boids.
    /// </summary>
    /// <param name="deltaV">change in velocity.</param>
    public void changeVelocity(Vector3 deltaV) {
        this.velocity += deltaV;
    }

    /// <summary>
    /// Method to determine direction of travel for a given boid.
    /// </summary>
    /// <returns>a vector3 representing the steer amount times the direction.</returns>
    public Vector3 steer() {
        Vector3 minDirection = Vector3.zero;
        float hitDistance = 0.5f;
        Transform hitTransform = null;

        Vector3[] rayDirections = BoidCollisionHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++) {
            Vector3 dir = transform.TransformDirection (rayDirections[i]);
            Ray ray = new Ray (position, dir);
            if (!Physics.SphereCast (ray, boundsRadius, collisionAvoidDst)) {
                return steerAmount * dir;
            }
        }
        print("Steering, couldn't decide where to go.");
        return new Vector3(0,0,0);
    }

    /// <summary>
    /// Method that determines if the boid will hit soon.
    /// </summary>
    /// <returns>true if it will collide, false otherwise.</returns>
    public bool nearCollision () {
        RaycastHit hit;
        if (Physics.SphereCast (position, boundsRadius, velocity, out hit, collisionAvoidDst)) {
            return true;
        } else {
        return false;
        }
    }

}
