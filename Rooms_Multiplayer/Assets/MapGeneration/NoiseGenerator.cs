using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NoiseGenerator for the map terrain (combines 3D Perlin Noise with Perlin Worms).
/// </summary>
public class NoiseGenerator : INoise {
    private int[] seed;

    int[,] noiseTable;
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float zMin;
    float zMax;

    int numStartWorms;
    int numWorms;

    public static int standardTunnelRadius = 15;
    public static float scale = 0.01f;

    /// <summary>
	/// Constructor for the noise generator.
	/// </summary>
	/// <param name="seed">array of 8 ints.</param>
	/// <param name="xMin">x for minimum in boundary.</param>
	/// <param name="xMax">x for maximum in boundary.</param>
	/// <param name="yMin">y for minimum in boundary.</param>
	/// <param name="yMax">y for maximum in boundary.</param>
	/// <param name="zMin">z for minimum in boundary.</param>
	/// <param name="zMax">z for maximum in bounadry.</param>
	/// <param name="numStartWorms">number of worms starting at 0, 0, 0.</param>
	/// <param name="numWorms">number of other worms in addition to numStartWorms.</param>
    public NoiseGenerator(int[] seed, float xMin, float xMax,
                          float yMin, float yMax, float zMin,
                          float zMax, int numStartWorms, int numWorms)
    {
        if (xMin >= xMax)
        {
            throw new System.ArgumentException("xMin should be less than xMax");
        }
        if (yMin >= yMax)
        {
            throw new System.ArgumentException("yMin should be less than yMax");
        }
        if (zMin >= zMax)
        {
            throw new System.ArgumentException("zMin should be less than zMax");
        }
        if (numStartWorms < 0)
        {
            throw new System.ArgumentException("numStartWorms must be non-negative");
        }
        if (numWorms < 0)
        {
            throw new System.ArgumentException("numWorms must be non-negative");
        }
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;
        this.zMin = zMin;
        this.zMax = zMax;
        this.seed = seed;
        this.numStartWorms = numStartWorms;
        this.numWorms = numWorms;
        // make the perlin worms
        PerlinWorms p = new PerlinWorms(seed, xMin, xMax, zMin, zMax, numStartWorms, numWorms);
        this.noiseTable = p.NoiseTable();
    }

    /// <summary>
	/// Getter method for the noise table.
	/// </summary>
	/// <returns>the noise table, an 2d int array</returns>
    public int[,] GetNoiseTable()
    {
        return this.noiseTable;
    }

    /// <summary>
	/// Method to calculate the 3d perlin noise of an x, y, z input.
	/// </summary>
	/// <param name="xInput">x coordinate as a float.</param>
	/// <param name="yInput">y coordinate as a float.</param>
	/// <param name="zInput">z coordinate as a float.</param>
	/// <returns>the 3d perlin noise of the coordinate as a float.</returns>
    public float PerlinNoise3D(float xInput, float yInput, float zInput) {
        float xInputScaled = (xInput * scale) + (seed[0] % 1000);
        float yInputScaled = (yInput * scale) + (seed[1] % 1000);
        float zInputScaled = (zInput * scale) + (seed[2] % 1000);
        float XY = Mathf.PerlinNoise(xInputScaled, yInputScaled);
        float YX = Mathf.PerlinNoise(yInputScaled, xInputScaled);
        float YZ = Mathf.PerlinNoise(yInputScaled, zInputScaled);
        float ZY = Mathf.PerlinNoise(zInputScaled, yInputScaled);
        float XZ = Mathf.PerlinNoise(xInputScaled, zInputScaled);
        float ZX = Mathf.PerlinNoise(zInputScaled, xInputScaled);
        float average = (XY + YX + YZ + ZY +  XZ + ZX)/6f;
        return average;
    }

    /// <summary>
	/// Method for calculating the noise of a given x, y, z coordinate.
	/// Combines 3d perlin noise with perlin worms to give cave-like structure.
	/// </summary>
	/// <param name="xInput">x coordinate as a float.</param>
	/// <param name="yInput">y coordinate as a float.</param>
	/// <param name="zInput">z coordinate as a float.</param>
	/// <returns>the noise of the 3d coordinate, as a float.</returns>
    public float Noise(float xInput, float yInput, float zInput)
    {
        // sets solid boundaries for the terrain
        if (xInput < this.xMin + 10 ||
            xInput > this.xMax - 10 ||
            zInput < this.zMin + 10 ||
            zInput > this.zMax - 10)
        {
            return -1;
        }

        int radiusOffset = Mathf.RoundToInt(Mathf.PerlinNoise(xInput * 0.05f, yInput * 0.05f) * standardTunnelRadius);
        int desiredTunnelRadius = radiusOffset + standardTunnelRadius;

        // calculate the radius and carve out the worms based on the noise table
        int radius = Mathf.RoundToInt(Mathf.Sqrt((desiredTunnelRadius * desiredTunnelRadius) - (yInput * yInput)));
        if (radius <= desiredTunnelRadius)
        {
            int xWithOffset = Mathf.RoundToInt(xInput - xMin);
            int zWithOffset = Mathf.RoundToInt(zInput - zMin);
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (xWithOffset + i >= 0
                        && xWithOffset + i < (xMax - xMin)
                        && zWithOffset + j >= 0
                        && zWithOffset + j < (zMax - zMin))
                    {
                        if (noiseTable[xWithOffset + i, zWithOffset + j] == 1)
                        {
                            return 1;
                        }
                    }
                }
            }
        }

        // if no worm at the point, calculate and return the 3d perlin noise.
        float noise = PerlinNoise3D(xInput, yInput, zInput);
        return noise;
    }
}
