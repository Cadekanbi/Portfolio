using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowerScript : MonoBehaviour
{
    // Variables
    public GameObject terrainRef;

    private Vector3 position;
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 moveMin; //Boundaries
    private Vector3 moveMax;

    private Bounds myBounds;

    private bool debugMode = false;

    public int pathNode;

    public float deltaForce = .0001f;
    public float deltaAngle = 1f;
    public float maxSpeed = 1f;
    public float drag = .95f;
    public float mass = 2f;
    public float coef = .2f;
    public float area;

    // Properties
    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }
    public bool DebugMode
    {
        get { return DebugMode; }
        set { debugMode = value; }
    }
    public int PathNode
    {
        get { return pathNode; }
        set { pathNode = value; }
    }

    // Methods
    // - Initialization
    void Start()
    {
        // Start in the place where you live
        position = gameObject.transform.position;
        direction = new Vector3 (0, 0, 1); // start pointing up
        velocity = new Vector3(0, 0, 0);

        area = myBounds.size.x * myBounds.size.y;
    }

    // - Fixed Update
    void FixedUpdate()
    {
        Slow();
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
        velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
        // Update position vector based on velocity
        position += velocity * Time.deltaTime;
        // Set height
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
        if (position.x >= 200)
        {
            position.x = 0;
        }
        if (position.z >= 200)
        {
            position.z = 0;
        }
    }

    // - Seek target path
    public void Seek(Vector3 targetPos)
    {
        Vector3 desiredVelocity = targetPos - transform.position;
        // Want to go toward THERE at max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        // Reynold's rule for steering
        Vector3 steeringForce = desiredVelocity - velocity;

        ApplyForce(steeringForce);
    }

    // Slow in water
    void Slow()
    {
        if (position.z < 91 && position.z > 73)
        {
            //Debug.Log("Slowed");
            maxSpeed = 2;
        }
        else
        {
            maxSpeed = 4;
        }
    }

    // Debug Code
    void DebugCode()
    {
        if (debugMode)
        {
            Debug.DrawLine(transform.position, transform.position + velocity);
        }
    }
}