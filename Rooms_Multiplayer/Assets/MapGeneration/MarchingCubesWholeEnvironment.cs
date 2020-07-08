using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for generating the terrain for the playing field.
/// </summary>
public class MarchingCubesWholeEnvironment : MonoBehaviour
{
    public static float xMin = -500;
    public static float xMax = 500;
    public static float yMin = -30;
    public static float yMax = 30;
    public static float zMin = -500;
    public static float zMax = 500;
    public static float edgeLength = 5;
    public static float densityThreshold = 0.5f;
    public int numStartWorms = 3;
    public int numWorms = 9;

    public GameObject mine;
    public GameObject boidGod;
    public GameObject terrainPiece;

    int numSchools = 50;

    public static int[,] noiseTable;

    // Start is called before the first frame update
    private void Start()
    {
        // check for the shared seed over the photon network
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Seed"))
        {
            Debug.LogError("Seed not found");
            return;
        }
        int[] seed = (int[])PhotonNetwork.CurrentRoom.CustomProperties["Seed"];
        NoiseGenerator sng = new NoiseGenerator(seed, xMin, xMax, yMin, yMax, zMin, zMax,
                                                numStartWorms, numWorms);
        MarchingCubesWholeEnvironment.noiseTable = sng.GetNoiseTable();
        MarchingCubes mc = new MarchingCubes(xMin, xMax, yMin, yMax, zMin, zMax,
                                             edgeLength, densityThreshold, sng);
        mc.Run();

        // place the treasure on the map
        GameObject t = GameObject.Find("Treasure");
        TreasureBehavior treasure = t.GetComponent<TreasureBehavior>();
        treasure.PlaceTreasure();
        // place the mines on the map
        ObjectPlacement.PlaceObjects(sng, mine, 20, seed);
        ObjectPlacement.PlaceObjects(sng, boidGod, numSchools, seed);
        Minimap.GenerateMinimapTerrain(sng, terrainPiece);
    }
}
