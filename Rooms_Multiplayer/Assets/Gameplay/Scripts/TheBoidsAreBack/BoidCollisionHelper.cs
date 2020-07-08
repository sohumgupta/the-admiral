using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class that contains math to calculate boid raycasting directions. Adapted from Sebastian Lague
/// </summary>
public static class BoidCollisionHelper
{
    const int numViewDirections = 300; //todo: changed from 3000
    public static readonly Vector3[] directions;

    /// <summary>
    /// Method that calculates necessary values for boid collision.
    /// </summary>
    static BoidCollisionHelper()
    {
        directions = new Vector3[BoidCollisionHelper.numViewDirections];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < numViewDirections; i++)
        {
            float t = (float)i / numViewDirections;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            directions[i] = new Vector3(x, y, z);
        }
    }
}