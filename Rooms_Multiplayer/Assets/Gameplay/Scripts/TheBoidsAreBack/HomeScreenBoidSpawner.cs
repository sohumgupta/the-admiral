using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for boids on the homescreen
/// </summary>
public class HomeScreenBoidSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject boidGod;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(boidGod,
                new Vector3(Random.Range(-50.0f, 50.0f),
                            Random.Range(0.0f, 20.0f),
                            Random.Range(-50.0f, 50.0f)),
                            Quaternion.Euler(0,Random.Range(0.0f, 360.0f),0));
                // new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            obj.transform.SetParent(GameObject.Find("BoidSpawner").transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
