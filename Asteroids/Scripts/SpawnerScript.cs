using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {

    // Variables
    public GameObject player;

    public Vector2 target;
    private int dir = 1;


    // Update
    void FixedUpdate()
    {
        // Update player position
        target = player.transform.position;
        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x); // Displacements
        // Rotate to point towards player
        transform.rotation = Quaternion.Euler(0, 0, (angle * 180) / Mathf.PI);
        //transform.Rotate(new Vector3(0, 0, 10));

        // Debugging - Draw vectors
        Debug.DrawLine(transform.position, target, Color.red);
        //Debug.DrawLine(transform.position, transform.rotation.eulerAngles * 3, Color.red);

        // Random Movement
        if (name == "SpawnerLeft" || name == "SpawnerRight")
        {
            // Movement of Spawners
            gameObject.transform.position += new Vector3(0, dir * Time.deltaTime, 0);
            
            // "Bounce"
            if (transform.position.y > 5)
            {
                dir = -1;
            }
            if (transform.position.y < -5)
            {
                dir = 1;
            }
        }
        else if (name == "SpawnerTop" || name == "SpawnerBottom")
        {
            // Movement of Spawners
            gameObject.transform.position += new Vector3(dir * Time.deltaTime, 0, 0);
            
            // "Bounce"
            if (transform.position.x > 8)
            {
                //Debug.Log("Switch");
                dir = -1;
            }
            if (transform.position.x < -8)
            {
                dir = 1;
            }
        }
	}
}
