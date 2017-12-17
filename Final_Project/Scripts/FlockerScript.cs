using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockerScript : MonoBehaviour
{
    // Variables
    private GameObject flockCenter;

    private float[][] field;

    private Vector3 position;
    private Vector3 direction;
    private Vector3 velocity;

    public bool debugMode = false;

    public float maxSpeed = 1f;
    public float mass = 2f;


    // Properties
    public GameObject FlockCenter
    {
        get { return FlockCenter; }
        set { flockCenter = value; }
    }
    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }
    public float[][] Field
    {
        get { return field; }
        set { field = value; }
    }
    public bool DebugMode
    {
        get { return DebugMode; }
        set { debugMode = value; }
    }

    // Methods
    // - Initialization
    void Start()
    {
        // Start in the place where you live
        position = gameObject.transform.position;
        direction = new Vector3(0, 0, 0); // start pointing up
        velocity = new Vector3(0, 0, 0);

    }

    // - Fixed Update
    void FixedUpdate()
    {
        FlockFollow();
        FixedUpdatePosition();
        KeepInBounds();
        transform.position = position; // Set position

        DebugCode();
    }
    // -- Fixed Update Helper Method
    void FixedUpdatePosition()
    {
        //Debug.Log("dir : " + direction);
        velocity += direction * Time.deltaTime;
        //Debug.Log("vel : " + velocity);
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        //Update position vector based on velocity
        position += velocity * Time.deltaTime;
        //Stay above the terrain
        position.y = Terrain.activeTerrain.SampleHeight(new Vector3(position.x, 0, position.z)) + 0.2f;
        //Debug.Log("pos : " + position);
        transform.rotation = Quaternion.Euler(0, -1 * Mathf.Rad2Deg * Mathf.Atan2(velocity.z, velocity.x) + 90, 0);
    }

    // - Apply Force
    public void ApplyForce(Vector3 aforce)
    {
        direction += aforce / mass; //accumulate force
    }

    // - Keep the GameObject in Bounds
    void KeepInBounds()
    {
        if (position.x > 200)
        {
            position.x = 0;
        }
        if (position.x < 0)
        {
            position.x = 200;
        }
        if (position.z > 200)
        {
            position.z = 0;
        }
        if (position.z < 0)
        {
            position.z = 200;
        }
    }

    // - Flock Follow
    void FlockFollow()
    {
        //If you get to far away dont just align to the velocity but get back to the flock
        Vector3 disp = transform.position - flockCenter.transform.position;
        if (disp.magnitude >= 10)
        {
            Seek(flockCenter.transform.position);
        }
    }

    // - Seek (For finding center)
    public void Seek(Vector3 targetPos)
    {
        Vector3 desiredVelocity = targetPos - transform.position;
        // Want to go toward THERE at max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        // Reynold's rule for steering
        Vector3 steeringForce = desiredVelocity - velocity;

        ApplyForce(steeringForce);
    }

    // - Align
    public void Align()
    {
        Vector3 targetVel = flockCenter.GetComponent<Flock>().Velocity;

        Vector3 desiredVelocity = targetVel + velocity;
        //Want to go toward THERE at max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        //Reynold's rule for steering
        Vector3 steeringForce = desiredVelocity - velocity;

        ApplyForce(steeringForce);
    }

    // - Debug Code
    void DebugCode()
    {
        if (debugMode)
        {
            Debug.DrawLine(transform.position, transform.position + velocity);
        }
    }
}