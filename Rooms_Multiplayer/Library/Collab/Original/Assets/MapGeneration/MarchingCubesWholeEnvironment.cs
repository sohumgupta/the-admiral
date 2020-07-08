using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubesWholeEnvironment : MonoBehaviour
{

    public static float xMin = -500;
    public static float xMax = 500;
    public static float yMin = -30;
    public static float yMax = 30;
    public static float zMin = -500;
    public static float zMax = 500;
    public float edgeLength = 5;
    public static float densityThreshold = 0.6f;

    public GameObject mine;
    public GameObject boidGod;

    int numSchools = 50;

    public static int[,] noiseTable;

    // Start is called before the first frame update
    private void Start()
    {
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Seed"))
        {
            Debug.LogError("SEED NOT FOUND OMG");
            return;
        }
        int[] seed = (int[])PhotonNetwork.CurrentRoom.CustomProperties["Seed"];
        // int[] seed = new int[]{0, 10, 20, 30, 40, 50, 60, 70};
        NoiseGenerator sng = new NoiseGenerator(seed, xMin, xMax, yMin, yMax, zMin, zMax);
        MarchingCubesWholeEnvironment.noiseTable = sng.getNoiseTable();
        MarchingCubes mc = new MarchingCubes(xMin, xMax, yMin, yMax, zMin, zMax,
                                             edgeLength, densityThreshold, sng);
        mc.renderCubes();
        GameObject t = GameObject.Find("Treasure");
        TreasureBehavior treasure = t.GetComponent<TreasureBehavior>();
        treasure.placeTreasure();

        ObjectPlacement.placeObjects(sng, mine, 20, seed);
        ObjectPlacement.placeObjects(sng, boidGod, numSchools, seed);
    }
}
