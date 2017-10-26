using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    // Variables
    public GameObject manager;

    private float speed;
    private double timer;

    private float hits = 0;

	// Initialization
	void Start ()
    {
        speed = manager.GetComponent<StatManager>().defaultBulletSpeed;
        timer = 0;
    }
	
	// Updates
    void FixedUpdate ()
    {
        // Move Forward
        if (gameObject.tag == "PulseBullet")
        {
            transform.position += transform.up.normalized * (speed / 1.5f);
        }
        else
        {
            transform.position += transform.up.normalized * (speed / 2);
        }
    }
	void Update ()
    {
        // Delete bullet after a set amount of time;
        timer += Time.deltaTime;
        if (timer > manager.GetComponent<StatManager>().bulletLifeTime)
        {
            Destroy(gameObject, 0);
        }
        // Or based on how many asteroids it destroyed/hit
        if ((hits >= 1 && gameObject.tag == "Bullet") || (hits >= 3 && gameObject.tag == "PulseBullet"))
        {
            Destroy(gameObject);
        }
    }

    // Collision
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Asteroid")
        {
            hits += 3;
            GameObject.Find("SceneManager").SendMessage("AddScore", 1);
        }
        else if (col.gameObject.tag == "SmallAsteroid")
        {
            hits++;
            GameObject.Find("SceneManager").SendMessage("AddScore", 2);
            // On asteroid collision script it will handle damage
        }
    }
}
