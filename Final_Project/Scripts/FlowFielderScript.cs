using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFielderScript : MonoBehaviour
{
    // Variables
    public GameObject terrainRef;

    private float[][] field;

    private Vector3 position;
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 moveMin; //Boundaries
    private Vector3 moveMax;

    private Bounds myBounds;

    public bool debugMode = false;

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

        //field = vectorField.GetComponent<FlowField>().field;

        area = myBounds.size.x * myBounds.size.y;
    }

    // - Fixed Update
    void FixedUpdate()
    {
        Flow();   
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
        // Update position vector based on velocity
        position += velocity * Time.deltaTime;
        // Stay above the terrain
        position.y = Terrain.activeTerrain.SampleHeight(new Vector3(position.x, 0, position.z)) + 0.2f;
        //Debug.Log("pos : " + position);
        transform.rotation = Quaternion.Euler(0, -1 * Mathf.Rad2Deg * Mathf.Atan2(velocity.z, velocity.x) - 90, 0);
    }

    // - Apply Force
    public void ApplyForce(Vector3 aforce)
    {
        direction += aforce / mass; //accumulate force
    }

    // - Keep the GameObject in Bounds ~~Also increases parallel speed the at a certain distance from edges~~
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

    // - Follow the flow of the vector field
    void Flow()
    {
        Vector3 targetVel = Vector3.zero;
        
        if (field != null && Mathf.FloorToInt(transform.position.x) < 200 && Mathf.FloorToInt(transform.position.x) >= 0 && Mathf.FloorToInt(transform.position.z) < 200 && Mathf.FloorToInt(transform.position.z) >= 0)
        {
            float slope = field[Mathf.FloorToInt(transform.position.x / 10)][Mathf.FloorToInt(transform.position.z / 10)];

            targetVel = new Vector3(1, 0, slope);
            targetVel.Normalize();
        }

        Vector3 desiredVelocity = targetVel + velocity;
        // Want to go toward THERE at max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        if (debugMode)
        {
            Debug.Log(desiredVelocity);
        }
        // Reynold's rule for steering
        Vector3 steeringForce = desiredVelocity - velocity;

        ApplyForce(steeringForce);
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