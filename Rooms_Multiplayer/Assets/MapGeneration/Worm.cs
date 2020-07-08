using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a Worm.
/// </summary>
public class Worm
{
    private Vector3[] vertices;

    private float segmentLength = 10;
    private int numSegments = 600;

    private float xOffset;
    private float zOffset;
    private float scale = 0.005f;
    private float offsetScale = 0.005f;
    private bool inverted;

    /// <summary>
	/// Constructor for a Worm.
	/// </summary>
	/// <param name="x">starting x coordinate.</param>
	/// <param name="xOffset">x offset for noise.</param>
	/// <param name="z">starting z coordinate.</param>
	/// <param name="zOffset">z offset for noise.</param>
	/// <param name="inverted">generate forward if true, backwards if false</param>
    public Worm(float x, float xOffset, float z, float zOffset, bool inverted)
    {
        this.inverted = inverted;
        this.vertices = new Vector3[numSegments];
        this.vertices[0] = new Vector3(x, 0, z);
        this.xOffset = xOffset * offsetScale;
        this.zOffset = zOffset * offsetScale;
    }

    /// <summary>
	/// Method to generate the vertices of the worm.
	/// </summary>
    public void GenerateVertices()
    {
        float[] radians = new float[numSegments];
        Vector3 startVertex = this.vertices[0];
        for (int i = 0; i < this.numSegments; i++)
        {
            float noise = Mathf.PerlinNoise(startVertex[0] + xOffset + (i * scale), startVertex[1] + zOffset);
            float angle = (noise - 0.5f) * 2*Mathf.PI;
            radians[i] = angle;
        }

        for (int i = 0; i < this.numSegments - 1; i++)
        {
            Vector3 currentVertex = this.vertices[i];
            Vector3 newVertex;
            // PerlinNoise centers around 0, so more than likely, the worm created will be in the forward direction. This let's us instead point it backwards.
            if (inverted)
            {
                newVertex = currentVertex - new Vector3(segmentLength * Mathf.Sin(radians[i]), 0, segmentLength * Mathf.Cos(radians[i]));
            } else {
                newVertex = currentVertex + new Vector3(segmentLength * Mathf.Sin(radians[i]), 0, segmentLength * Mathf.Cos(radians[i]));
            }
            vertices[i + 1] = newVertex;
        }
    }

    /// <summary>
	/// Getter method for the coordinates of the worm.
	/// </summary>
	/// <returns>a vector3 array representing the vertices of the worm</returns>
    public Vector3[] GetVertices()
    {
        return this.vertices;
    }
}
