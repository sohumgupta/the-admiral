using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Script for placing objects on the map.
/// </summary>
public class ObjectPlacement : MonoBehaviour
{
    /// <summary>
	/// Method to place an object on the map.
	/// </summary>
	/// <param name="generator">INoise used for 3d noise method.</param>
	/// <param name="obj">GameObject to place.</param>
	/// <param name="numObjects">Number of objects to place.</param>
	/// <param name="seed">array of 8 ints.</param>
    public static void PlaceObjects(INoise generator, GameObject obj, int numObjects, int[] seed)
    {
        int range = 1000;
        int randomSeed = seed[0] % range;

        float xMax = MarchingCubesWholeEnvironment.xMax;
        float xMin = MarchingCubesWholeEnvironment.xMin;
        float yMax = MarchingCubesWholeEnvironment.yMax - 10;
        float yMin = MarchingCubesWholeEnvironment.yMin + 15;
        float zMax = MarchingCubesWholeEnvironment.zMax;
        float zMin = MarchingCubesWholeEnvironment.zMin;

        Random.seed = randomSeed;

        int objectsPlaced = 0;

        while (objectsPlaced < numObjects)
        {
            float xCoord = (Random.value * 2) - 1;
            if (xCoord < 0) { xCoord *= -xMin; } else { xCoord *= xMax;}

            float yCoord = (Random.value * 2) - 1;
            if (yCoord < 0) { yCoord *= -yMin; } else { yCoord *= yMax; }

            float zCoord = (Random.value * 2) - 1;
            if (zCoord < 0) { zCoord *= -zMin; } else { zCoord *= zMax; }

            if (generator.Noise(xCoord, yCoord, zCoord) > MarchingCubesWholeEnvironment.densityThreshold)
            {
                Instantiate(obj, new Vector3(xCoord, yCoord, zCoord), Quaternion.Euler(0, 0, 0));
                objectsPlaced = objectsPlaced + 1;
            }
        }
    }
}
