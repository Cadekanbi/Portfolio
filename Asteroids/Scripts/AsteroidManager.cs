using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour {

    // Variables
    public GameObject smallAsteroid;
    public GameObject asteroid;

    public GameObject leftSpawner;
    public GameObject rightSpawner;
    public GameObject topSpawner;
    public GameObject bottomSpawner;

    private Vector2 displacement;

    private float spawnTimes = 7;
    private float timer1;
    private float timer2;

	// Use this for initialization
	void Start () {

        timer1 = 0;
        timer2 = 0;

        Instantiate(asteroid, leftSpawner.transform.position, leftSpawner.transform.rotation);
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (timer1 >= spawnTimes)
        {
            float num = Random.Range(0f, 4f);
            if (num > 3)
            {
                Instantiate(asteroid, leftSpawner.transform.position, leftSpawner.transform.rotation);
            }
            else if (num > 2)
            {
                Instantiate(asteroid, rightSpawner.transform.position, rightSpawner.transform.rotation);
            }
            else if (num > 1)
            {
                Instantiate(asteroid, topSpawner.transform.position, topSpawner.transform.rotation);
            }
            else
            {
                Instantiate(asteroid, bottomSpawner.transform.position, bottomSpawner.transform.rotation);
            }

            timer1 = 0;
            Debug.Log("Asteroid Spawned");
        }

        if (timer2 >= (spawnTimes * 2))
        {
            float num = Random.Range(0f, 4f);
            if (num > 3)
            {
                Instantiate(smallAsteroid, leftSpawner.transform.position, leftSpawner.transform.rotation);
            }
            else if (num > 2)
            {
                Instantiate(smallAsteroid, rightSpawner.transform.position, rightSpawner.transform.rotation);
            }
            else if (num > 1)
            {
                Instantiate(smallAsteroid, topSpawner.transform.position, topSpawner.transform.rotation);
            }
            else
            {
                Instantiate(smallAsteroid, bottomSpawner.transform.position, bottomSpawner.transform.rotation);
            }

            timer2 = 0;
            Debug.Log("Small Asteroid Spawned");
        }
    }
}
