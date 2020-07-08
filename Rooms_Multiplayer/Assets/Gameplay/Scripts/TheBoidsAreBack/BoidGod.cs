using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class BoidGod that controls 1 school of fish.
/// </summary>
public class BoidGod : MonoBehaviour
{

    public GameObject daFish;

    //start constants
    int numBoids = 50; //was 20
    int startDist = 2;
    int startVel = 1;
    Vector3 startCenter;
    float startingRange = 200; //was 100

    //cohesion constant
    float cohesion = 0.03f;

    //separation constants
    float DIST_THRESHOLD = .8f;
    float avoidance = .4f;

    //alignment constant
    float alignment = 0.005f;

    List<Boid> boids;

    Vector3 direction;
    Vector3 center;

    float boidSizeVariance = .4f;
    float boidSizeMean;

    //scatter
    bool scatterBool = false; //determines if fish scatter occasionally or not
    float minTime = 4f;
    float maxTime = 7f;
    float time;
    float scatterTime;

    //strong wind/current
    bool windBool = false; //determines if wind or not
    float windAmount = 3;
    bool wind = false;
    float windTime;
    Vector3 windy = new Vector3(0,0,0);

    // Start is called before the first frame update
    void Start() {

        boids = new List<Boid>();

        time = 0;
        scatterTime = Random.Range(minTime, maxTime);
        windTime = Random.Range(8f, 14f);

        startCenter = this.transform.position; //boid gods are placed around the map

        boidSizeMean = Random.Range(.9f - boidSizeVariance, .9f + boidSizeVariance);



        for(int i = 0; i < numBoids; i++) {
            //create boids
            var boid = Instantiate(daFish, new Vector3(Random.Range(
                -1 * startDist,startDist) + startCenter[0], Random.Range(-1 * startDist,startDist)
                + startCenter[1], Random.Range(-1 * startDist,startDist) + startCenter[2]),
                Quaternion.identity);

            Boid boidTyped = boid.GetComponent(typeof(Boid)) as Boid;

            boidTyped.setId(i);
            boidTyped.changeVelocity(new Vector3(0,0,5));
            boidTyped.setScale(boidSizeMean);

            boids.Add(boidTyped);
        }
    }

    /// <summary>
    /// Method that executes the separation principle for boids.
    /// </summary>
    /// <param name="b">a boid (fish).</param>
    /// <returns></returns>
    Vector3 separate(Boid b) {
        Vector3 v = new Vector3(0,0,0);
        foreach (Boid boid in boids) {
            if (boid.id != b.id) {
                if (Vector3.Distance(b.position, boid.position) < DIST_THRESHOLD) {
                    v += avoidance * (b.position - boid.position);
                }
            }
        }
        return v;
    }

    /// <summary>
    /// Method that executes the cohesion principle for boids.
    /// </summary>
    /// <param name="b">a boid (fish).</param>
    /// <returns></returns>
    Vector3 cohere(Boid b) {
        Vector3 center = new Vector3(0,0,0);
        foreach (Boid boid in boids) {
            if (boid.id != b.id) {
                center += boid.getPosition();
            }
        }
        center = center / numBoids;

        return cohesion * (center - b.getPosition());
    }

    /// <summary>
    /// Method that executes the alignment principle for boids.
    /// </summary>
    /// <param name="b">a boid (fish).</param>
    /// <returns></returns>
    Vector3 align(Boid b) {
        Vector3 v = new Vector3(0,0,0);
        foreach (Boid boid in boids) {
            if (boid.id != b.id) {
                v += boid.getVelocity();
            }
        }
        v = v / numBoids;
        return alignment * (v - b.getVelocity());
    }

    /// <summary>
    /// Method that controls behavior for scattering the boids.
    /// </summary>
    void scatter() {
        cohesion = -1 * Mathf.Abs(cohesion);
    }

    /// <summary>
    /// Method that controls behavior for unscattering the boids.
    /// </summary>
    void unscatter() {
        cohesion = 1 * Mathf.Abs(cohesion);
    }

    /// <summary>
    /// Method that controls behaviors for winds/currents.
    /// </summary>
    /// <returns></returns>
    Vector3 strongWind() {
        return new Vector3(Random.Range(-1 * windAmount, windAmount),
            Random.Range(-1 * windAmount, windAmount), Random.Range(-1 * windAmount, windAmount));
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        //for each boid, use the 3 rules to determine the boid's velocity
        foreach (Boid boid in boids) {
            Vector3 v = new Vector3(0,0,0);

            v += separate(boid);
            v += cohere(boid);
            v += align(boid);
            if (wind) {
                v += windy;
            }
            if (boid.nearCollision()) {
                v += boid.steer();
            }

            boid.changeVelocity(v);
            boid.clamp();
            boid.updateRotation();
            boid.changePosition();
        }
        if (scatterBool) {
            if ((time > scatterTime) && (time < scatterTime + 1)) {
            scatter();
        }
            if (time > (scatterTime + 1)) {
                unscatter();
                time = 0;
            }
        }

        if (windBool) {
            if (time > windTime) {
                if (windy == new Vector3(0,0,0)) {
                    windy = strongWind();
                }
                wind = true;

            } if (time > windTime + 1) {
                wind = false;
                windy = new Vector3(0,0,0);
                time = 0;
            }

        }
    }
}
