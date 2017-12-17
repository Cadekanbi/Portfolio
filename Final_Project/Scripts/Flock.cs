using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Variables
    public Camera activeCam;
    public Terrain terrainRef;

    public GameObject[] flockers;
    public GameObject flockerPrefab;
    public GameObject cursorPrefab;
    public GameObject cursor;
    public int numOfFlockers = 8;

    private Vector3 position;
    private Vector3 velocity;

    private bool mouseOver = false;
    private bool mousePress = false;
    public bool debugMode = false;

    private int cursorCount = 1;

    private float count = 0;
    public float maxSpeed = 1f;
    public float mass = 2f;


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


    // Methods
    // - Initialization
    void Start()
    {
        activeCam = Camera.allCameras[0];
        terrainRef = Terrain.activeTerrain;

        //Base vector values
        position = gameObject.transform.position;
        velocity = new Vector3(0.5f, 0, 2);

        // Create the Flockers
        flockers = new GameObject[numOfFlockers];
        for (int i = 0; i < numOfFlockers; i++)
        {
            Vector3 rand = new Vector3(Random.Range(10, 20), 0, Random.Range(175, 190));
            GameObject flocker = Instantiate(flockerPrefab, rand, transform.rotation);
            flocker.GetComponent<FlockerScript>().FlockCenter = gameObject;
            flockers[i] = flocker;
        }
    }

    // - Update
    void Update()
    {
        //Get active camera
        activeCam = Camera.main;

        //Mouse Press
        mousePress = Input.GetMouseButton(0);
        //If the mouse is hovering somewhere over the terrain
        Ray ray = activeCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            mouseOver = true;
        }
        else
        {
            mouseOver = false;
        }

        //Do something special (indicator)
        if (mouseOver)
        {
            //If you click
            if (mousePress)
            {
                //Create 1 instance
                if (cursorCount == 1)
                {
                    cursor = Instantiate(cursorPrefab, hit.point, transform.rotation);
                }

                //Continuously follow Raycast Hit each frame
                if (cursor != null)
                {
                    Vector3 tempVect = hit.point;
                    tempVect.y = Terrain.activeTerrain.SampleHeight(new Vector3(hit.point.x, 0, hit.point.z)) + 2;
                    cursor.transform.position = tempVect;
                    // Change flocker center position
                    position = tempVect;
                }

                //Make sure you the cursor once
                if (cursorCount > 0)
                {
                    cursorCount--;
                }
            }
            else
            {
                //Delete that instance if it exists and you're not clicking 
                if (cursor != null)
                {
                    Destroy(cursor);
                }

                //Reset Count
                cursorCount = 1;
            }
        }
        //Delete instance in case you drag over the edge
        else
        {
            if (cursor != null)
            {
                Destroy(cursor);
            }
        }
    }

    // - Fixed Update
    void FixedUpdate()
    {
        //Debug.Log(mouseOver + " and " + mousePress);
        if (!(mousePress && mouseOver))
        {
            count += Time.deltaTime;
            if (count > 100)
            {
                count = 0;
            }
            //Set Velocity
            if (count <= 25)
            {
                velocity = new Vector3(1, 0, 1);
            }
            else if (count <= 50)
            {
                velocity = new Vector3(1, 0, -1);
            }
            else if (count <= 75)
            {
                velocity = new Vector3(-1, 0, -1);
            }
            else
            {
                velocity = new Vector3(-1, 0, 1);
            }

            FixedUpdatePosition();
        }
        Separation();
        Flocking();

        transform.position = position; // Set position

        DebugCode();
    }
    // -- Fixed Update Helper Method
    void FixedUpdatePosition()
    {
        //Position based on flockers 
        Vector3 avgPos = Vector3.zero;
        foreach (GameObject flocker in flockers)
        {
            avgPos += flocker.transform.position;
        }
        avgPos /= flockers.Length;
        position = avgPos;
        position.y = Terrain.activeTerrain.SampleHeight(new Vector3(position.x, 0, position.z)) + 2;
        //Debug.Log("pos : " + position);
        transform.rotation = Quaternion.Euler(0, -1 * Mathf.Rad2Deg * Mathf.Atan2(velocity.z, velocity.x) + 90, 0);
    }

    // - Flock
    void Flocking()
    {
        foreach (GameObject flocker in flockers)
        {
            flocker.GetComponent<FlockerScript>().Align();
        }
    }

    // - Separate Flockers
    void Separation()
    {
        foreach (GameObject flocker in flockers)
        {
            foreach (GameObject otherFlocker in flockers)
            {
                if (!flocker.Equals(otherFlocker))
                {
                    Vector3 disp = (flocker.transform.position - otherFlocker.transform.position) * -1;
                    if (disp.magnitude < 2.5f)
                    {
                        flocker.GetComponent<FlockerScript>().ApplyForce(disp);
                    }
                }
            }
        }
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