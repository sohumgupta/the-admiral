using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controls behavior of the treasure, mainly treasure placement.
/// </summary>
public class TreasureBehavior : MonoBehaviour
{
    PlayerMovement player;
    private float distanceToWin = 5f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
	/// Method to place the treasure on a path.
	/// </summary>
    public void PlaceTreasure()
    {
        int[,] noiseTable = MarchingCubesWholeEnvironment.noiseTable;
        int xDim = noiseTable.GetLength(0);
        int yDim = noiseTable.GetLength(1);
        int startingOffset = 50;
        for (int offset = startingOffset; offset < xDim - startingOffset; offset++)
        {
            for (int j = offset; j < yDim - offset; j++)
            {
                if (noiseTable[offset, j] == 1)
                {
                    transform.position = new Vector3(offset - 500, transform.position.y, j - 500);
                    return;
                }
                if (noiseTable[xDim - offset, j] == 1)
                {
                    transform.position = new Vector3((xDim - offset) - 500, transform.position.y, j - 500);
                    return;
                }
            }
            for (int i = offset; i < xDim - offset; i++)
            {
                if (noiseTable[i, offset] == 1)
                {
                    transform.position = new Vector3(i - 500, transform.position.y, offset - 500);
                    return;
                }
                if (noiseTable[i, yDim - offset] == 1)
                {
                    transform.position = new Vector3(i - 100, transform.position.y, (yDim - offset) - 500);
                    return;
                }
            }
        }
    }
}
