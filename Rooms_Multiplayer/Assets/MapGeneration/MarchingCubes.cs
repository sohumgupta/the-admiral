using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class MarchingCubes that executes the MarchingCubes algorithm.
/// </summary>
public class MarchingCubes
{
    // fields that denote the boundaries
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float zMin;
    float zMax;

    // fields for edge length of cube and density threshold
    float edgeLength;
    float densityThreshold;

    // fields to hold the cubes and triangles made
    List<Cube> cubes;
    List<Triangle> triangles;

    // field to hold the INoise
    INoise n;

    // mesh limit on number of vertices
    int meshLimit = 65535;

    /// <summary>
	/// Constructor for the Marching Cubes class.
	/// </summary>
	/// <param name="xMin">minimum x value for the boundary.</param>
	/// <param name="xMax">maximum x value for the boundary.</param>
	/// <param name="yMin">minimum y value for the boundary.</param>
	/// <param name="yMax">maximum y value for the boundary.</param>
	/// <param name="zMin">minimum z value for the boundary.</param>
	/// <param name="zMax">maximum z value for the boundary.</param>
	/// <param name="edgeLength">edge length for each cube's edge</param>
	/// <param name="densityThreshold">density threshold to be used with noise function.</param>
	/// <param name="n">INoise thats used for the 3d noise.</param>
    public MarchingCubes(float xMin, float xMax, float yMin, float yMax,
                         float zMin, float zMax, float edgeLength,
                         float densityThreshold, INoise n)
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
        if (edgeLength <= 0)
        {
            throw new System.ArgumentException("edge length should be less than 0");
        }
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;
        this.zMin = zMin;
        this.zMax = zMax;
        this.edgeLength = edgeLength;
        this.densityThreshold = densityThreshold;
        this.n = n;
    }

    /// <summary>
	/// Static method to randomly generate a seed (int array with 8 elements).
	/// </summary>
	/// <returns>an int array of 8 elements</returns>
    public static int[] GenerateSeed()
    {
        System.Random rand = new System.Random();
        int[] seed = new int[8];
        for (int i = 0; i < 8; i++)
        {
            seed[i] = rand.Next();
        }
        return seed;
    }

    /// <summary>
	/// Method to run the algorithm (makes the cubes, triangles, and renders the mesh).
	/// </summary>
    public void Run()
	{
        this.MakeCubesAndTriangles();
        this.RenderCubes();
	}

    /// <summary>
	/// Method to make the cubes and triangles.
	/// </summary>
    private void MakeCubesAndTriangles()
	{
        this.cubes = new List<Cube>();
        this.triangles = new List<Triangle>();

        //instantiate all of the cubes
        for (float x = xMin; x <= xMax; x += edgeLength)
        {
            for (float y = yMin; y <= yMax; y += edgeLength)
            {
                for (float z = zMin; z <= zMax; z += edgeLength)
                {
                    Cube cube = new Cube(edgeLength, x, y, z);
                    foreach (Vertex v in cube.GetVertices())
                    {
                        float density = n.Noise(v.GetX(), v.GetY(), v.GetZ());
                        v.SetDensity(density);
                    }
                    this.cubes.Add(cube);
                }
            }
        }
        foreach (Cube cube in cubes)
        {
            Vertex[] vertices = cube.GetVertices();
            // finding the appropriate index of the cube to look up in the triangulation table provided in SebLague's github repo
            int cubeIndex = 0;
            int powerOfTwo = 1;
            for (int i = 0; i < 8; i++)
            {
                if (vertices[i].GetDensity() < densityThreshold) cubeIndex |= powerOfTwo;
                powerOfTwo = powerOfTwo * 2;
            }
            int[] edges = Tables.triangulation[cubeIndex];
            // adding the triangles
            for (int i = 0; i < 16 && edges[i] != -1; i += 3)
            {
                Triangle triangle;
                int cornerIndexA1 = Tables.cornerIndexAFromEdge[edges[i]];
                int cornerIndexB1 = Tables.cornerIndexBFromEdge[edges[i]];
                triangle.a = vertices[cornerIndexA1].GetMidPoint(vertices[cornerIndexB1]);
                int cornerIndexA2 = Tables.cornerIndexAFromEdge[edges[i + 1]];
                int cornerIndexB2 = Tables.cornerIndexBFromEdge[edges[i + 1]];
                triangle.b = vertices[cornerIndexA2].GetMidPoint(vertices[cornerIndexB2]);
                int cornerIndexA3 = Tables.cornerIndexAFromEdge[edges[i + 2]];
                int cornerIndexB3 = Tables.cornerIndexBFromEdge[edges[i + 2]];
                triangle.c = vertices[cornerIndexA3].GetMidPoint(vertices[cornerIndexB3]);
                triangles.Add(triangle);
            }
        }
    }

    /// <summary>
	/// Method to render the cubes after making the cubes and triangles.
	/// </summary>
    private void RenderCubes()
    {
        Vector3[] verticesToRender = new Vector3[meshLimit];
        int[] indicesToRender = new int[meshLimit];

        // loop for adding each triangle's vertices
        int counter = 0;
        foreach(Triangle t in this.triangles)
        {
            if (counter >= meshLimit)
            {
                Chunk chunk = new Chunk();
                chunk.Render(verticesToRender, indicesToRender);
                verticesToRender = new Vector3[meshLimit];
                indicesToRender = new int[meshLimit];
                counter = 0;
            }
            Vector3[] verticesOfT = new Vector3[]{t.a, t.b, t.c};
            int[] clockwiseIndicies = new int[]{0, 1, 2};
            foreach(int i in clockwiseIndicies)
            {
                verticesToRender[counter + i] = verticesOfT[i];
                indicesToRender[counter + i] = counter + i;
            }
            counter += 3;
        }
        // if there's any leftover vertices, render those too
        if (counter != 0)
        {
            Chunk chunk = new Chunk();
            chunk.Render(verticesToRender, indicesToRender);
        }
    }
}

/// <summary>
/// Struct for a triangle (it just holds 3 Vector3s).
/// </summary>
struct Triangle {
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;
}
