using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for Minimap.
/// </summary>
public class Minimap : MonoBehaviour
{
    PlayerMovement player;
    MarchingCubesWholeEnvironment cubes;

    Color emptyColor = new Color(1, 1, 1, 0f);
    Color filledColor = new Color(1, 1, 1, 1);

    void Start() {
        player = FindObjectOfType<PlayerMovement>();
        cubes = FindObjectOfType<MarchingCubesWholeEnvironment>();
    }

    /// <summary>
    /// Method to generate terrain in minimap display.
    /// </summary>
    public static void GenerateMinimapTerrain(INoise noiseGenerator, GameObject terrainPiece)
    {
        int xMax = Mathf.RoundToInt(MarchingCubesWholeEnvironment.xMax);
        int xMin = Mathf.RoundToInt(MarchingCubesWholeEnvironment.xMin);
        int zMax = Mathf.RoundToInt(MarchingCubesWholeEnvironment.zMax);
        int zMin = Mathf.RoundToInt(MarchingCubesWholeEnvironment.zMin);
        int edgeLength = Mathf.RoundToInt(MarchingCubesWholeEnvironment.edgeLength);

        GameObject minimapTerrain = GameObject.Find("MinimapTerrain");

        for (int x = xMin; x < xMax; x += edgeLength)
        {
            for (int z = zMin; z < zMax; z += edgeLength)
            {
                if (noiseGenerator.Noise(x, 0, z) < MarchingCubesWholeEnvironment.densityThreshold)
                {
                    GameObject piece = Instantiate(terrainPiece, new Vector3(x, 0, z), Quaternion.Euler(90, 0, 0));
                    piece.transform.SetParent(minimapTerrain.transform);
                }
            }
        }
    }
}
