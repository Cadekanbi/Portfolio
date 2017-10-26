using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Cobey Adekanbi
 * 10/16/17
 */

public class ShipPhysics : MonoBehaviour
{

    // Vars
    // ======

    // ~ Settings
    private Renderer[] renderers;

    // ~ GameObejcts
    public StatManager manager;
    public Camera cam;
    public GameObject thrusters;
    public GameObject gun;

    // ~ Physics
    private Vector2 position;
    private Vector2 velocity;
    public Vector2 Velocity
    {
        get { return velocity; }
    }
    private Vector2 acceleration;

    public float maxVel;
    public float maxAcc;
    private float decelConst = 0.0005f;

    private float deltaAngle = 0;
    public float Angle
    {
        get { return deltaAngle; }
    }
    private float rotationSpeed = 2;

    // ~ Effects
    private bool isWrappingX = false;
    private bool isWrappingY = false;
    private float currentShipFloat = 0.01f;
    const float defaultFloatVal = 0.015f;
    const float shipBrakeVal = 0.005f;

    // Methods
    // =========
    void Start()
    {
        // Screen wrap concept from --> https://www.youtube.com/watch?v=3uI8qXDCmzU
        renderers = GetComponentsInChildren<Renderer>();

        maxVel = manager.GetComponent<StatManager>().defaultMaxVel;
        maxAcc = manager.GetComponent<StatManager>().defaultMaxAcc;

        position = transform.position;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        deltaAngle = 90;
    }

    void FixedUpdate()
    {
        // ================= Debugging ===================
        // ===============================================
        //Debug.Log(acceleration);
        //Debug.Log("X: " + camWidth + ", Y: " + camHeight);
        // ===============================================

        // ===============================================
        
        // Change rotation
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            // increase
            deltaAngle+= rotationSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            // decrease
            deltaAngle-= rotationSpeed;
        }

        // ===============================================

        // Control acceleration
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            acceleration += new Vector2(0.05f * Time.deltaTime, 0.05f * Time.deltaTime);
            rotationSpeed = 2;
        }
        else
        {
            if (velocity.x > currentShipFloat)
            {
                acceleration.x -= decelConst;
            }
            else
            {
                acceleration.x = 0;
                velocity.x = currentShipFloat;
            }
            if (velocity.y > currentShipFloat)
            {
                acceleration.y -= decelConst;
            }
            else
            {
                acceleration.y = 0;
                velocity.y = currentShipFloat;
            }

            // Allow for faster rotation when not accelerating
            rotationSpeed = 5;
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            currentShipFloat = shipBrakeVal;
        }
        else
        {
            currentShipFloat = defaultFloatVal;
        }

        // ===============================================

        // Max Velocity and Acceleration
        if (acceleration.x > maxAcc)
        {
            acceleration.x = maxAcc;
        }
        if (acceleration.y > maxAcc)
        {
            acceleration.y = maxAcc;
        }

        if (velocity.x > maxVel)
        {
            velocity.x = maxVel;
            acceleration.x = 0;
        }
        if (velocity.y > maxVel)
        {
            velocity.y = maxVel;
            acceleration.y = 0;
        }

        // ===============================================

        // Screen Wrap
        ScreenWrap();
        
        // ===============================================

        // Alter rotation
        transform.rotation = Quaternion.Euler(0, 0, deltaAngle - 90);

        // Alter velocity
        velocity += acceleration;

        // Alter position
        position += new Vector2(velocity.x * Mathf.Cos(deltaAngle * Mathf.Deg2Rad), velocity.y * Mathf.Sin(deltaAngle * Mathf.Deg2Rad));
        // Position Rotation - position = new Vector2(velocity.x * Mathf.Cos(deltaAngle * Mathf.Deg2Rad), velocity.y * Mathf.Sin(deltaAngle * Mathf.Deg2Rad));

        // Position will equal the pos. vector every method call
        gameObject.transform.position = position;

        // ===============================================

        // Position
        thrusters.transform.position = position;
        thrusters.transform.rotation = Quaternion.Euler(0, 0, deltaAngle - 90);
        gun.transform.position = position;
        gun.transform.rotation = Quaternion.Euler(0, 0, deltaAngle - 90);
    }

    void ScreenWrap()
    {
        bool isVisible = CheckRenderers();

        // If the ship is on screen then don't wrap
        if (isVisible)
        {
            thrusters.SetActive(true);
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingX && isWrappingY)
        {
            return;
        }

        Vector2 newPos = position;
        if (newPos.x > 1 || newPos.x < 0)
        {
            thrusters.SetActive(false);
            newPos.x = -newPos.x;
            isWrappingX = true;
        }
        if (newPos.y > 1 || newPos.y < 0)
        {
            thrusters.SetActive(false);
            newPos.y = -newPos.y;
            isWrappingX = true;
        }

        position = newPos;
    }

    // Checks to see if player is "on screen"
    bool CheckRenderers()
    {
        foreach(Renderer renderer in renderers)
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }

        return false;
    }
}
