using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that instantiates a number of BoidGods (each of which control 1 school).
/// </summary>
public class BoidMetaGod : MonoBehaviour
{

    public GameObject BoidGod;
    int numSchools = 50;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numSchools; i++) {
            var boidGod = Instantiate(BoidGod);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
