using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the PerlinWorms algorithm.
/// </summary>
public class PerlinWorms
{
    // initial position
    float xPos = 0f;
    float zPos = 0f;

    float xOffset;
    float zOffset;

    // number of worms guaranteed to start at 0, 0, 0
    int numStartWorms;

    // number of worms generated, not including the starting worms
    int numWorms;

    Worm[] worms;
    int[] seed;

    int xSeedIndex = 0;
    int zSeedIndex = 0;

    int xMin;
    int xMax;
    int zMin;
    int zMax;
    int xRange;
    int zRange;

    /// <summary>
	/// Constructor for the PerlinWorms class.
	/// </summary>
	/// <param name="seed">array of 8 ints.</param>
	/// <param name="xMin">x minimum for boundary.</param>
	/// <param name="xMax">x maximum for boundary.</param>
	/// <param name="zMin">z minimum for boundary.</param>
	/// <param name="zMax">z maximum for boundary.</param>
	/// <param name="numStartWorms">number of worms staring at 0, 0, 0.</param>
	/// <param name="numWorms">number of other worms.</param>
    public PerlinWorms(int[] seed, float xMin, float xMax,
                       float zMin, float zMax, int numStartWorms, int numWorms)
    {
        if (xMin >= xMax)
        {
            throw new System.ArgumentException("xMin should be less than xMax");
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
        this.xMin = Mathf.RoundToInt(xMin);
        this.xMax = Mathf.RoundToInt(xMax);
        this.zMin = Mathf.RoundToInt(zMin);
        this.zMax = Mathf.RoundToInt(zMax);
        this.xRange = this.xMax - this.xMin;
        this.zRange = this.zMax - this.zMin;

        this.numStartWorms = numStartWorms;
        this.numWorms = numWorms;

        int[] moddedSeed = new int[seed.Length];
        for (int i = 0; i < seed.Length; i++)
        {
            moddedSeed[i] = (seed[i] % xRange) + this.xMin;
        }
        this.seed = moddedSeed;
        this.CreateWorms();
    }

    /// <summary>
	/// Incrememnt the indices for pseudo-random worm generation.
	/// </summary>
    private void IncrementSeedIndices()
    {
        int temp = seed[xSeedIndex];
        seed[xSeedIndex] = seed[zSeedIndex];
        seed[zSeedIndex] = temp;

        xSeedIndex += 1;
        // if x index is out of bounds, reset to 0 and increment z index
        if (xSeedIndex >= seed.Length)
        {
            xSeedIndex = 0;
            zSeedIndex += 1;
        }
        // if z index is out of bounds, reset both back to 0
        if (zSeedIndex >= seed.Length)
        {
            xSeedIndex = 0;
            zSeedIndex = 0;
        }
    }

    /// <summary>
	/// Method to create all of the worms.
	/// </summary>
	/// <returns>an array of all the worms created.</returns>
    private void CreateWorms()
    {
        worms = new Worm[numStartWorms + numWorms];

        bool currInverted = false;
        // paths starting from initial position
        for (int i = 0; i < numStartWorms; i++)
        {
            xOffset = seed[xSeedIndex];
            zOffset = seed[zSeedIndex];
            Worm worm = new Worm(xPos, xOffset, zPos, zOffset, currInverted);
            currInverted = !currInverted;
            worm.GenerateVertices();
            worms[i] = worm;
            IncrementSeedIndices();
        }

        for (int i = 0; i < numWorms; i++)
        {
            xOffset = seed[xSeedIndex];
            zOffset = seed[zSeedIndex];

            IncrementSeedIndices();

            xPos = seed[xSeedIndex];
            zPos = seed[zSeedIndex];

            Worm worm = new Worm(xPos, xOffset, zPos, zOffset, currInverted);
            currInverted = !currInverted;
            worm.GenerateVertices();
            worms[i + numStartWorms] = worm;
            IncrementSeedIndices();
        }
    }

    public Worm[] GetWorms()
    {
        return this.worms;
    }

    /// <summary>
	/// Method to draw a worm. Used mostly for testing purposes.
	/// </summary>
	/// <param name="worm">worm object to draw.</param>
    public void DrawWorm(Worm worm) {
        Vector3[] vertices = worm.GetVertices();
        Vector3 start;
        Vector3 end;
        for (int i = 0; i < vertices.Length - 1; i++)
        {
            start = vertices[i];
            end = vertices[i + 1];
            GameObject myLine = new GameObject();
            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            lr.startColor = Color.red;
            lr.endColor = Color.red;
            lr.startWidth = 1f;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            Material mat = new Material(Shader.Find("Unlit/Texture"));
            lr.material = mat;
        }
    }

    /// <summary>
	/// Method to make the noise table with the worms.
	/// </summary>
	/// <returns>a 2d array of ints, where 1 means there's a worm, and 0 otherwise.</returns>
    public int[,] NoiseTable()
    {
        int[,] table = new int[xRange, zRange];
        foreach(Worm worm in worms)
        {
            Vector3[] vertices = worm.GetVertices();
            foreach(Vector3 vertex in vertices) {
                int x = Mathf.RoundToInt(vertex[0]) - xMin;
                int z = Mathf.RoundToInt(vertex[2]) - zMin;
                if (x >= 0 && x < xRange && z >= 0 && z < zRange)
                {
                    table[x, z] = 1;
                }
            }
        }
        return table;
    }
}
