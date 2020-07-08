using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a Cube that holds all 8 coordinates of vertices.
/// </summary>
public class Cube
{

    // Coordinates of the vertices, in this order, using the following notation scheme:
    // U = Up or positive y direction, D = Down or negative y direction
    // R = Right or positive x direction, L = Left or negative x direction
    // F = Front or positive z direction, B = Back or negative z direction
    // DLB, DRB, DRF, DLF, ULB, URB, URF, ULF
    private Vertex[] vertices;

    /// <summary>
	/// Constructor for a cube, given edge length and the coordinates of the DLB vertex,
	/// and calculates the rest of them.
	/// </summary>
	/// <param name="edgeLength">float representing the edge length of the cube</param>
	/// <param name="x">x coordinate of the DLB vertex.</param>
	/// <param name="y">y coordinate of the DLB vertex.</param>
	/// <param name="z">z coordinate of the DLB vertex.</param>
    public Cube(float edgeLength, float x, float y, float z)
    {
        if (edgeLength <= 0)
        {
            throw new System.ArgumentException("edge length must be greater than 0");
        }
        Vertex[] newVertices = new Vertex[8];
        newVertices[0] = new Vertex(x, y, z);
        newVertices[1] = new Vertex(x + edgeLength, y, z);
        newVertices[2] = new Vertex(x + edgeLength, y, z + edgeLength);
        newVertices[3] = new Vertex(x, y, z + edgeLength);
        newVertices[4] = new Vertex(x, y + edgeLength, z);
        newVertices[5] = new Vertex(x + edgeLength, y + edgeLength, z);
        newVertices[6] = new Vertex(x + edgeLength, y + edgeLength, z + edgeLength);
        newVertices[7] = new Vertex(x, y + edgeLength, z + edgeLength);
        this.vertices = newVertices;
    }

    /// <summary>
	/// Getter method for the cube's vertices.
	/// </summary>
	/// <returns>A vertex array of the 8 vertices.</returns>
    public Vertex[] GetVertices()
    {
        return this.vertices;
    }
}
