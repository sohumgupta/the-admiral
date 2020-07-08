using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for a class that has a 3d noise function.
/// </summary>
public interface INoise
{
    /// <summary>
	/// Method that returns the noise (float) given a 3d coordinate.
	/// </summary>
	/// <param name="xInput">x coordinate as a float</param>
	/// <param name="yInput">y coordinate as a float</param>
	/// <param name="zInput">z coordinate as a float</param>
	/// <returns>the noise at the 3d coordinate specified by x, y, and z</returns>
    float Noise(float xInput, float yInput, float zInput);
}
