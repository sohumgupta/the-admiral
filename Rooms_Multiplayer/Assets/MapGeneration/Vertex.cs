using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to hold the coordinates of a Vertex
/// </summary>
public class Vertex
{
    private float x;
    private float y;
    private float z;
    private float density;

    /// <summary>
	/// Constructor for Vertex.
	/// </summary>
	/// <param name="a">x coordinate of vertex.</param>
	/// <param name="b">y coordinate of vertex.</param>
	/// <param name="c">z coordinate of vertex.</param>
    public Vertex(float a, float b, float c)
    {
        this.x = a;
        this.y = b;
        this.z = c;
        //default value will be 0 for the density
        this.density = 0;
    }

    /// <summary>
	/// Getter method for x coordinate.
	/// </summary>
	/// <returns>x coordinate of vertex.</returns>
    public float GetX()
    {
        return this.x;
    }

    /// <summary>
	/// Getter method for y coordinate.
	/// </summary>
	/// <returns>y coordinate of vertex.</returns>
    public float GetY()
    {
        return this.y;
    }

    /// <summary>
	/// Getter method for z coordinate.
	/// </summary>
	/// <returns>z coordinate of vertex.</returns>
    public float GetZ()
    {
        return this.z;
    }

    /// <summary>
	/// Setter method for density of vertex.
	/// </summary>
	/// <param name="density">density of the vertex.</param>
    public void SetDensity(float density)
    {
        this.density = density;
    }

    /// <summary>
	/// Getter method for density of vertex.
	/// </summary>
	/// <returns>density of the vertex.</returns>
    public float GetDensity()
    {
        return this.density;
    }

    /// <summary>
	/// Method to make a Vector3 inbetween two vertices, this and another vertex
	/// </summary>
	/// <param name="otherVertex">other vertex to find the distance between.</param>
	/// <returns>a Vector3, the midpoint between the two vertices.</returns>
    public Vector3 GetMidPoint(Vertex otherVertex)
    {
        return new Vector3((this.x + otherVertex.GetX()) * 0.5f,
                          (this.y + otherVertex.GetY()) * 0.5f,
                          (this.z + otherVertex.GetZ()) * 0.5f);
    }

    public override bool Equals(object obj)
    {
        return obj is Vertex vertex &&
               x == vertex.x &&
               y == vertex.y &&
               z == vertex.z;
    }

    public override int GetHashCode()
    {
        int hashCode = 373119288;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        hashCode = hashCode * -1521134295 + z.GetHashCode();
        return hashCode;
    }
}
